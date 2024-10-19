package com.dwin.du.api.service;
import com.dwin.du.api.dto.ProductDto;
import com.dwin.du.api.entity.Product;
import com.dwin.du.api.repository.ProductRepository;
import com.dwin.du.api.request.AddEditProductRequest;
import com.dwin.du.api.entity.PersonProduct;
import com.dwin.du.api.repository.PersonProductRepository;
import com.dwin.du.api.entity.Receipt;
import com.dwin.du.api.repository.ReceiptRepository;
import com.dwin.du.api.entity.User;
import com.dwin.du.update.EntityUpdateService;
import com.dwin.du.validation.ValidationService;
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
    private final ValidationService validator;
    private final EntityUpdateService updater;

    public ResponseEntity<?> addProduct(String username, int receiptId, AddEditProductRequest request) {
        User user = validator.validateUser(username);
        validator.isNull(request, "Nie przekazano danych");
        validator.isEmpty(request.getName(), "Nazwa produktu jest wymagana");
        validator.isNull(request.getPrice(), "Cena jest wymagana");
        validator.isNull(request.getMaxQuantity(), "Maksymalna ilość jest wymagana");
        validator.isNull(request.isDivisible(), "Informacja o podzielności jest wymagana");
        validator.isNull(receiptId, "Brak identyfikatora rachunku");
        Receipt receipt = validator.validateReceipt(username, receiptId);

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
        updater.updateTotalPriceReceipt(receipt);
        return ResponseEntity.ok(product);
    }

    public ResponseEntity<?> editProduct(String username, int productId, AddEditProductRequest request) {
        validator.validateUser(username);
        validator.isNull(request, "Nie przekazano danych");
        validator.isEmpty(request.getName(), "Nazwa produktu jest wymagana");
        validator.isNull(request.getPrice(), "Cena jest wymagana");
        validator.isNull(request.getMaxQuantity(), "Maksymalna ilość jest wymagana");
        validator.isNull(request.isDivisible(), "Informacja o podzielności jest wymagana");
        validator.isNull(productId, "Brak identyfikatora produktu");
        Product product = validator.validateProduct(username, productId);

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
        updater.updatePartPricesPersonProduct(product);
        updater.updateCompensationPrice(product);
        updater.updateTotalPriceReceipt(product.getReceipt());
        updater.updatePerson(username);
        return ResponseEntity.ok(product);
    }

    public ResponseEntity<?> removeProduct(String username, int productId) {
        validator.validateUser(username);
        validator.isNull(productId, "Brak identyfikatora produktu");
        Product product = validator.validateProduct(username, productId);
        Receipt receipt = validator.validateReceipt(username, product.getReceipt().getId());

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);

        personProductRepository.deleteAll(personProducts);
        productRepository.delete(product);
        updater.updateTotalPriceReceipt(receipt);
        updater.updatePerson(username);
        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> setIsSettled(String username, int productId, boolean settled) {
        validator.validateUser(username);
        validator.isNull(productId, "Brak identyfikatora produktu");
        Product product = validator.validateProduct(username, productId);

        product.setSettled(settled);
        productRepository.save(product);

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        for (PersonProduct personProduct : personProducts) {
            personProduct.setSettled(settled);
            personProductRepository.save(personProduct);
        }

        Receipt receipt = product.getReceipt();
        boolean allSettled = updater.areAllProductsSettled(receipt);
        receipt.setSettled(allSettled);

        receiptRepository.save(receipt);
        updater.updatePerson(username);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getProduct(String username, int productId) {
        validator.validateUser(username);
        validator.isNull(productId, "Brak identyfikatora produktu");
        Product product = validator.validateProduct(username, productId);

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

    public ResponseEntity<?> getProductsFromReceipt(String username, int receiptId) {
        validator.validateUser(username);
        validator.isNull(receiptId, "Brak identyfikatora rachunku");
        Receipt receipt = validator.validateReceipt(username, receiptId);

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
}