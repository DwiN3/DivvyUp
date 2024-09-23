package com.dwin.du.entity.product;


import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person.PersonRepository;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Request.AddProductRequest;
import com.dwin.du.entity.product.Response.ShowProductResponse;
import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.receipt.ReceiptRepository;
import com.dwin.du.entity.receipt.Request.SetIsSettledRequest;
import com.dwin.du.security.user.User;
import com.dwin.du.security.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.*;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class ProductService {

    private final ReceiptRepository receiptRepository;
    private final UserRepository userRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;
    private final PersonRepository personRepository;

    public ResponseEntity<?> addProductToReceipt(AddProductRequest request, int receiptId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();


        Product product = Product.builder()
                .receipt(receipt)
                .productName(request.getProductName())
                .price(request.getPrice())
                .packagePrice(request.getPackagePrice())
                .divisible(request.isDivisible())
                .maxQuantity(request.getMaxQuantity())
                .compensationAmount(0)
                .isSettled(receipt.isSettled())
                .build();

        if(product.isDivisible())
            product.setMaxQuantity(request.getMaxQuantity());
        else
            product.setMaxQuantity(1);

        productRepository.save(product);
        updateReceiptTotalAmount(receipt);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removeProduct(int productId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent())
            return ResponseEntity.notFound().build();

        Product product = optionalProduct.get();
        if (!product.getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Receipt receipt = product.getReceipt();

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        personProductRepository.deleteAll(personProducts);
        productRepository.delete(product);
        updateBalancesAfterProductChange(personProducts);

        updateReceiptTotalAmount(receipt);

        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> setIsSettled(int productId, SetIsSettledRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent())
            return ResponseEntity.notFound().build();

        Product product = optionalProduct.get();
        if (!product.getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        product.setSettled(request.isSettled());
        productRepository.save(product);

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        for (PersonProduct personProduct : personProducts) {
            personProduct.setSettled(request.isSettled());
            personProductRepository.save(personProduct);
        }

        updateBalancesAfterProductChange(personProducts);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> showProductById(int productId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent())
            return ResponseEntity.notFound().build();

        Product product = optionalProduct.get();
        if (!product.getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        ShowProductResponse response = ShowProductResponse.builder()
                .productId(product.getProductId())
                .receiptId(product.getReceipt().getReceiptId())
                .productName(product.getProductName())
                .price(product.getPrice())
                .compensationAmount(product.getCompensationAmount())
                .packagePrice(product.getPackagePrice())
                .divisible(product.isDivisible())
                .maxQuantity(product.getMaxQuantity())
                .isSettled(product.isSettled())
                .build();
        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> showAllProductsFromReceipt(int receiptId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        List<Product> products = productRepository.findByReceipt(optionalReceipt.get());
        List<ShowProductResponse> responseList = new ArrayList<>();
        for (Product product : products) {
            ShowProductResponse response = ShowProductResponse.builder()
                    .productId(product.getProductId())
                    .receiptId(product.getReceipt().getReceiptId())
                    .productName(product.getProductName())
                    .price(product.getPrice())
                    .compensationAmount(product.getCompensationAmount())
                    .packagePrice(product.getPackagePrice())
                    .divisible(product.isDivisible())
                    .maxQuantity(product.getMaxQuantity())
                    .isSettled(product.isSettled())
                    .build();
            responseList.add(response);
        }
        return ResponseEntity.ok(responseList);
    }

    private void updateBalancesAfterProductChange(List<PersonProduct> personProducts) {
        Set<Person> involvedPersons = personProducts.stream()
                .map(PersonProduct::getPerson)
                .collect(Collectors.toSet());

        for (Person person : involvedPersons) {
            updateTotalPurchaseAmountForPerson(person);
        }
    }

    public void updateTotalPurchaseAmountForPerson(Person person) {
        List<PersonProduct> personProducts = personProductRepository.findByPerson(person);

        double totalPurchaseAmount = 0.0;

        for (PersonProduct personProduct : personProducts) {
            if (!personProduct.isSettled()) {
                double partOfPrice = personProduct.getPartOfPrice();
                totalPurchaseAmount += partOfPrice;

                if(personProduct.isCompensation()){
                    totalPurchaseAmount += personProduct.getProduct().getCompensationAmount();
                }
            }
        }

        BigDecimal compensationRounded = new BigDecimal(totalPurchaseAmount).setScale(2, RoundingMode.UP);
        person.setTotalPurchaseAmount(compensationRounded.doubleValue());
        personRepository.save(person);
    }

    private void updateReceiptTotalAmount(Receipt receipt) {
        List<Product> products = productRepository.findByReceipt(receipt);
        double totalAmount = products.stream().mapToDouble(Product::getPrice).sum();

        receipt.setTotalAmount(totalAmount);
        receiptRepository.save(receipt);
    }
}