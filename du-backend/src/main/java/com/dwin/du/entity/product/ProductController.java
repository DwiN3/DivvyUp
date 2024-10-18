package com.dwin.du.entity.product;

import com.dwin.du.entity.product.Request.AddEditProductRequest;
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

    @PostMapping("/receipt/{receiptId}/product/add")
    @Operation(summary = "Add a product to a receipt", description = "Adds a new product to a specific receipt.")
    public ResponseEntity<?> addProduct(@PathVariable int receiptId, @RequestBody AddEditProductRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return productService.addProduct(username, receiptId, request);
    }

    @PutMapping("/product/{productId}/edit")
    @Operation(summary = "Edit a product", description = "Edits the details of an existing product by its ID.")
    public ResponseEntity<?> editProduct(@PathVariable int productId, @RequestBody AddEditProductRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return productService.editProduct(username, productId, request);
    }

    @DeleteMapping("/product/{productId}/remove")
    @Operation(summary = "Remove a product", description = "Removes a product by its ID.")
    public ResponseEntity<?> removeProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return productService.removeProduct(username, productId);
    }

    @PutMapping("/product/{productId}/set-settled")
    @Operation(summary = "Mark product as settled", description = "Marks a product as settled.")
    public ResponseEntity<?> setIsSettled(@PathVariable int productId, @RequestParam boolean settled){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return productService.setIsSettled(username, productId, settled);
    }

    @GetMapping("/product/{productId}")
    @Operation(summary = "Retrieve a product", description = "Retrieves the details of a product by its ID.")
    public ResponseEntity<?> getProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return productService.getProduct(username, productId);
    }

    @GetMapping("/receipt/{receiptId}/product")
    @Operation(summary = "Retrieve all products in a receipt", description = "Retrieves all products associated with a specific receipt.")
    public ResponseEntity<?> getProductsFromReceipt(@PathVariable int receiptId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return productService.getProductsFromReceipt(username,receiptId);
    }
}