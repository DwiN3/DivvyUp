package com.dwin.du.entity.product;
import com.dwin.du.entity.product.Request.AddEditProductRequest;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.receipt.ReceiptRepository;
import com.dwin.du.entity.user.User;
import com.dwin.du.service.EntityUpdateService;
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
        validator.isNull(request);
        validator.isEmpty(request.getName());
        validator.isNull(request.getPrice());
        validator.isNull(request.getMaxQuantity());
        validator.isNull(request.isDivisible());
        validator.isNull(receiptId);
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
        validator.isNull(request);
        validator.isEmpty(request.getName());
        validator.isNull(request.getPrice());
        validator.isNull(request.getMaxQuantity());
        validator.isNull(request.isDivisible());
        validator.isNull(productId);
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
        validator.isNull(productId);
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
        validator.isNull(productId);
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
        validator.isNull(productId);
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
        validator.isNull(receiptId);
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