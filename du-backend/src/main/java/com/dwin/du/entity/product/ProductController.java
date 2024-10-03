package com.dwin.du.entity.product;

import com.dwin.du.entity.receipt.ReceiptDto;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm")
@Tag(name = "Product Management", description = "APIs for managing products within receipts")
public class ProductController {

    private final ProductService productService;

    @PostMapping("/receipt/{receiptID}/product/add")
    @Operation(summary = "Add a product to a receipt", description = "Adds a new product to a specific receipt.")
    public ResponseEntity<?> addProductToReceipt(@PathVariable int receiptID, @RequestBody ProductDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.addProductToReceipt(request, receiptID, currentUsername);
    }

    @PutMapping("/product/edit/{productId}")
    @Operation(summary = "Edit a product", description = "Edits an existing product by ID.")
    public ResponseEntity<?> editProduct(@PathVariable int productId, @RequestBody ReceiptDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.editProduct(productId, request, currentUsername);
    }

    @DeleteMapping("/product/remove/{productId}")
    @Operation(summary = "Remove a product", description = "Removes a product by ID.")
    public ResponseEntity<?> removeProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.removeProduct(productId, currentUsername);
    }

    @PutMapping("/product/set-settled/{productId}")
    @Operation(summary = "Mark product as settled", description = "Marks a product as settled.")
    public ResponseEntity<?> setIsSettled(@PathVariable int productId, @RequestBody ProductDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.setIsSettled(productId, request, currentUsername);
    }

    @GetMapping("/product/{productId}")
    @Operation(summary = "Get a product", description = "Retrieves a product by ID.")
    public ResponseEntity<?> getProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.getProductById(productId, currentUsername);
    }

    @GetMapping("/receipt/{receiptID}/product")
    @Operation(summary = "Get all products in a receipt", description = "Retrieves all products within a specific receipt.")
    public ResponseEntity<?> getProducts(@PathVariable int receiptID) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.getProductsFromReceipt(receiptID, currentUsername);
    }
}