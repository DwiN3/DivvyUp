package com.dwin.rm.entity.product;


import com.dwin.rm.entity.product.Request.AddProductRequest;
import com.dwin.rm.entity.product.Response.ShowProductResponse;
import com.dwin.rm.entity.receipt.Receipt;
import com.dwin.rm.entity.receipt.ReceiptRepository;
import com.dwin.rm.entity.receipt.Request.SetIsSettledRequest;
import com.dwin.rm.security.user.User;
import com.dwin.rm.security.user.UserRepository;
import io.jsonwebtoken.MalformedJwtException;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class ProductService {
    private final ProductRepository productRepository;
    private final ReceiptRepository receiptRepository;
    private final UserRepository userRepository;

    private ResponseEntity<?> checkUser(String username) {
        try {
            Optional<User> optionalUser = userRepository.findByUsername(username);
            if (!optionalUser.isPresent()) {
                return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
            }
        } catch (MalformedJwtException e) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        return null;
    }

    private ResponseEntity<?> checkReceiptOwnership(int receiptId, String username) {
        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        return null;
    }

    public ResponseEntity<?> addProductToReceipt(AddProductRequest request, int receiptId, String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

        ResponseEntity<?> receiptOwnershipCheckResponse = checkReceiptOwnership(receiptId, username);
        if (receiptOwnershipCheckResponse != null)
            return receiptOwnershipCheckResponse;

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        Product product = Product.builder()
                .receipt(receipt)
                .productName(request.getProductName())
                .price(request.getPrice())
                .packagePrice(request.getPackagePrice())
                .divisible(request.isDivisible())
                .maxQuantity(request.getMaxQuantity())
                .isSettled(false)
                .build();
        productRepository.save(product);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removeProduct(int productId, String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent())
            return ResponseEntity.notFound().build();

        Product product = optionalProduct.get();
        ResponseEntity<?> receiptOwnershipCheckResponse = checkReceiptOwnership(product.getReceipt().getReceiptId(), username);
        if (receiptOwnershipCheckResponse != null)
            return receiptOwnershipCheckResponse;

        productRepository.delete(product);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsSettled(int productId, SetIsSettledRequest request, String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent())
            return ResponseEntity.notFound().build();

        Product product = optionalProduct.get();
        ResponseEntity<?> receiptOwnershipCheckResponse = checkReceiptOwnership(product.getReceipt().getReceiptId(), username);
        if (receiptOwnershipCheckResponse != null)
            return receiptOwnershipCheckResponse;

        product.setSettled(request.isSettled());
        productRepository.save(product);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> showProductById(int productId, String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent())
            return ResponseEntity.notFound().build();

        Product product = optionalProduct.get();
        ResponseEntity<?> receiptOwnershipCheckResponse = checkReceiptOwnership(product.getReceipt().getReceiptId(), username);
        if (receiptOwnershipCheckResponse != null)
            return receiptOwnershipCheckResponse;

        ShowProductResponse response = ShowProductResponse.builder()
                .productId(product.getProductId())
                .receipt(product.getReceipt().getReceiptId())
                .productName(product.getProductName())
                .price(product.getPrice())
                .packagePrice(product.getPackagePrice())
                .divisible(product.isDivisible())
                .maxQuantity(product.getMaxQuantity())
                .isSettled(product.isSettled())
                .build();
        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> showAllProductsFromReceipt(int receiptId, String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

        ResponseEntity<?> receiptOwnershipCheckResponse = checkReceiptOwnership(receiptId, username);
        if (receiptOwnershipCheckResponse != null)
            return receiptOwnershipCheckResponse;

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        List<Product> products = productRepository.findByReceipt(optionalReceipt.get());
        List<ShowProductResponse> responseList = new ArrayList<>();
        for (Product product : products) {
            ShowProductResponse response = ShowProductResponse.builder()
                    .productId(product.getProductId())
                    .receipt(product.getReceipt().getReceiptId())
                    .productName(product.getProductName())
                    .price(product.getPrice())
                    .packagePrice(product.getPackagePrice())
                    .divisible(product.isDivisible())
                    .maxQuantity(product.getMaxQuantity())
                    .isSettled(product.isSettled())
                    .build();
            responseList.add(response);
        }
        return ResponseEntity.ok(responseList);
    }
}