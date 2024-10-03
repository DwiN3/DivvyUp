package com.dwin.du.entity.product;

import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.receipt.ReceiptRepository;
import com.dwin.du.entity.user.User;
import com.dwin.du.entity.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
@RequiredArgsConstructor
public class ProductService {

    private final UserRepository userRepository;
    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;

    public ResponseEntity<?> addProductToReceipt(ProductDto request, int receiptId, String username) {
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
                .name(request.getName())
                .price(request.getPrice())
                .divisible(request.isDivisible())
                .maxQuantity(request.getMaxQuantity())
                .compensationPrice(0)
                .isSettled(receipt.isSettled())
                .build();

        if(product.isDivisible())
            product.setMaxQuantity(request.getMaxQuantity());
        else
            product.setMaxQuantity(1);

        productRepository.save(product);

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

        productRepository.delete(product);

        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> setIsSettled(int productId, ProductDto request, String username) {
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

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getProductById(int productId, String username) {
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

        ProductDto response = ProductDto.builder()
                .id(product.getId())
                .receiptId(product.getReceipt().getId())
                .name(product.getName())
                .price(product.getPrice())
                .compensationPrice(product.getCompensationPrice())
                .divisible(product.isDivisible())
                .maxQuantity(product.getMaxQuantity())
                .isSettled(product.isSettled())
                .build();

        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getProductsFromReceipt(int receiptId, String username) {
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
                    .isSettled(product.isSettled())
                    .build();
            responseList.add(response);
        }

        return ResponseEntity.ok(responseList);
    }
}