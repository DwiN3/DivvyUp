package com.dwin.du.entity.product;

import com.dwin.du.entity.product.Request.AddProductRequest;
import com.dwin.du.entity.product.Request.EditProductRequest;
import com.dwin.du.entity.receipt.ReceiptDto;
import com.dwin.du.entity.receipt.Request.SetSettledRequest;
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
    public ResponseEntity<?> addProductToReceipt(@PathVariable int receiptId, @RequestBody AddProductRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.addProductToReceipt(request, receiptId, currentUsername);
    }

    @PutMapping("/product/edit/{id}")
    @Operation(summary = "Edit a product", description = "Edits an existing product by ID.")
    public ResponseEntity<?> editProduct(@PathVariable int id, @RequestBody EditProductRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.editProduct(id, request, currentUsername);
    }

    @DeleteMapping("/product/remove/{id}")
    @Operation(summary = "Remove a product", description = "Removes a product by ID.")
    public ResponseEntity<?> removeProduct(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.removeProduct(id, currentUsername);
    }

    @PutMapping("/product/set-settled/{id}")
    @Operation(summary = "Mark product as settled", description = "Marks a product as settled.")
    public ResponseEntity<?> setIsSettled(@PathVariable int id, @RequestBody SetSettledRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.setIsSettled(id, request, currentUsername);
    }

    @GetMapping("/product/{id}")
    @Operation(summary = "Get a product", description = "Retrieves a product by ID.")
    public ResponseEntity<?> getProduct(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.getProductById(id, currentUsername);
    }

    @GetMapping("/receipt/{receiptId}/product")
    @Operation(summary = "Get all products in a receipt", description = "Retrieves all products within a specific receipt.")
    public ResponseEntity<?> getProducts(@PathVariable int receiptId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.getProductsFromReceipt(receiptId, currentUsername);
    }
}