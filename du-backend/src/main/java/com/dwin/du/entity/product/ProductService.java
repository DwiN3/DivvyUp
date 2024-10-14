package com.dwin.du.entity.product;

import com.dwin.du.entity.person.PersonUpdateService;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Request.AddProductRequest;
import com.dwin.du.entity.product.Request.EditProductRequest;
import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.receipt.ReceiptRepository;
import com.dwin.du.entity.user.User;
import com.dwin.du.valid.ValidService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
@RequiredArgsConstructor
public class ProductService {

    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;
    private final ValidService valid;
    private final PersonUpdateService updatePerson;

    public ResponseEntity<?> addProductToReceipt(AddProductRequest request, int receiptId, String username) {
        User user = valid.validateUser(username);
        Receipt receipt = valid.validateReceipt(username, receiptId);

        Product product = Product.builder()
                .user(user)
                .receipt(receipt)
                .name(request.getName())
                .price(request.getPrice())
                .divisible(request.isDivisible())
                .maxQuantity(request.getMaxQuantity())
                .settled(receipt.isSettled())
                .build();

        if(product.isDivisible()) {
            product.setMaxQuantity(request.getMaxQuantity());
            product.setCompensationPrice(request.getPrice());
        }
        else {
            product.setMaxQuantity(1);
            product.setCompensationPrice(0);
        }

        productRepository.save(product);
        updateTotalPriceReceipt(receipt);

        return ResponseEntity.ok(product);
    }

    public ResponseEntity<?> editProduct(int productId, EditProductRequest request, String username) {
        valid.validateUser(username);
        Product product = valid.validateProduct(username, productId);

        if(product.isDivisible() && !request.isDivisible()){
            List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
            personProductRepository.deleteAll(personProducts);
        }

        product.setName(request.getName());
        product.setPrice(request.getPrice());
        product.setDivisible(request.isDivisible());

        if(request.isDivisible()) {
            product.setMaxQuantity(request.getMaxQuantity());
            product.setCompensationPrice(request.getPrice());
        }
        else {
            product.setMaxQuantity(1);
            product.setCompensationPrice(0);
        }

        productRepository.save(product);
        updatePerson.updateAllData(username);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removeProduct(int productId, String username) {
        valid.validateUser(username);
        Product product = valid.validateProduct(username, productId);
        Receipt receipt = valid.validateReceipt(username, product.getReceipt().getId());

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        personProductRepository.deleteAll(personProducts);
        productRepository.delete(product);

        updateTotalPriceReceipt(receipt);
        updatePerson.updateAmounts(username);

        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> setIsSettled(int productId, boolean settled, String username) {
        valid.validateUser(username);
        Product product = valid.validateProduct(username, productId);

        product.setSettled(settled);
        productRepository.save(product);

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        for (PersonProduct personProduct : personProducts) {
            personProduct.setSettled(settled);
            personProductRepository.save(personProduct);
        }

        Receipt receipt = product.getReceipt();
        boolean allSettled = areAllProductsSettled(receipt);
        receipt.setSettled(allSettled);
        receiptRepository.save(receipt);
        updatePerson.updateUnpaidAmount(username);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getProductById(int productId, String username) {
        valid.validateUser(username);
        Product product = valid.validateProduct(username, productId);

        ProductDto response = ProductDto.builder()
                .id(product.getId())
                .receiptId(product.getReceipt().getId())
                .name(product.getName())
                .price(product.getPrice())
                .compensationPrice(product.getCompensationPrice())
                .divisible(product.isDivisible())
                .maxQuantity(product.getMaxQuantity())
                .settled(product.isSettled())
                .build();

        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getProductsFromReceipt(int receiptId, String username) {
        valid.validateUser(username);
        Receipt receipt = valid.validateReceipt(username, receiptId);

        List<Product> products = productRepository.findByReceipt(receipt);

        List<ProductDto> responseList = new ArrayList<>();
        for (Product product : products) {
            ProductDto response = ProductDto.builder()
                    .id(product.getId())
                    .receiptId(product.getReceipt().getId())
                    .name(product.getName())
                    .price(product.getPrice())
                    .compensationPrice(product.getCompensationPrice())
                    .divisible(product.isDivisible())
                    .maxQuantity(product.getMaxQuantity())
                    .settled(product.isSettled())
                    .build();
            responseList.add(response);
        }

        return ResponseEntity.ok(responseList);
    }

    private void updateTotalPriceReceipt(Receipt receipt) {
        List<Product> products = productRepository.findByReceipt(receipt);
        double totalPrice = 0.00;
        for(var item : products)
            totalPrice += item.getPrice();

        receipt.setTotalPrice(totalPrice);
        receiptRepository.save(receipt);
    }

    private boolean areAllProductsSettled(Receipt receipt) {
        List<Product> products = productRepository.findByReceipt(receipt);
        return products.stream().allMatch(Product::isSettled);
    }
}