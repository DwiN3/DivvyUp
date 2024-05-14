package com.dwin.rm.entity.product;


import com.dwin.rm.entity.person_product.PersonProduct;
import com.dwin.rm.entity.person_product.PersonProductRepository;
import com.dwin.rm.entity.person_product.PersonProductService;
import com.dwin.rm.entity.product.Request.AddProductRequest;
import com.dwin.rm.entity.receipt.ReceiptRepository;
import com.dwin.rm.security.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class ProductService {
   /*
    private final UserRepository userRepository;
    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;

    public ResponseEntity<?> addProductToReceipt(int receiptId, AddProductRequest request, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        var receipts = receiptRepository.findAllByAddedByUserId(user.getUserId());
        boolean isOwner = receipts.stream().anyMatch(receipt -> receipt.getReceiptId() == receiptId);
        if (!isOwner) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }

        var product = Product.builder()
                .receiptId(receiptId)
                .productName(request.getProductName())
                .price(request.getPrice())
                .packagePrice(request.getPackagePrice())
                .divisible(request.isDivisible())
                .maxQuantity(request.getMaxQuantity())
                .build();

        productRepository.save(product);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> editProduct(int productId, AddProductRequest request, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        var productOptional = productRepository.findById(productId);
        if (productOptional.isEmpty()) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
        var product = productOptional.get();
        // TODO: Zabezpieczenie czy użytkonik z tokena jest właścicielem

        if (request.getProductName() != null) {
            product.setProductName(request.getProductName());
        }
        if (request.getPrice() != null) {
            product.setPrice(request.getPrice());
            }
            product.setPackagePrice(request.getPackagePrice());
            product.setDivisible(request.isDivisible());
            product.setMaxQuantity(request.getMaxQuantity());

        productRepository.save(product);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removeProduct(int productId, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        var productOptional = productRepository.findById(productId);
        if (productOptional.isEmpty()) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }

        var product = productOptional.get();
        // TODO: Zabezpieczenie czy użytkonik z tokena jest właścicielem

        List<PersonProduct> personProducts = personProductRepository.findAllByProductId(productId);
        personProductRepository.deleteAll(personProducts);
        
        productRepository.delete(product);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> showProduct(int productId, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        var productOptional = productRepository.findById(productId);
        if (productOptional.isEmpty()) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }

        var product = productOptional.get();
        // TODO: Zabezpieczenie czy użytkonik z tokena jest właścicielem

        return ResponseEntity.ok(product);
    }

    public ResponseEntity<?> showAllProductsFromReceipt(int receiptId, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        var receipts = receiptRepository.findAllByAddedByUserId(user.getUserId());
        boolean isOwner = receipts.stream().anyMatch(receipt -> receipt.getReceiptId() == receiptId);
        if (!isOwner) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }

        List<Product> allProducts = productRepository.findAllByReceiptId(receiptId);

        return ResponseEntity.ok(allProducts);
    }*/
}