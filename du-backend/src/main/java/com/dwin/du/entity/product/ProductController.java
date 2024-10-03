package com.dwin.du.entity.product;

import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm")
public class ProductController {

    private final ProductService productService;

    @PostMapping("/receipt/{receiptID}/product/add")
    public ResponseEntity<?> addProductToReceipt(@PathVariable int receiptID, @RequestBody ProductDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.addProductToReceipt(request, receiptID, currentUsername);
    }

    @DeleteMapping("/product/remove/{productId}")
    public ResponseEntity<?> removeProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.removeProduct(productId, currentUsername);
    }

    @PutMapping("/product/set-is-settled/{productId}")
    public ResponseEntity<?> setIsSettled(@PathVariable int productId, @RequestBody ProductDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.setIsSettled(productId, request, currentUsername);
    }

    @GetMapping("/product/{productId}")
    public ResponseEntity<?> getProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.getProductById(productId, currentUsername);
    }

    @GetMapping("/receipt/{receiptID}/product")
    public ResponseEntity<?> getProducts(@PathVariable int receiptID) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.getProductsFromReceipt(receiptID, currentUsername);
    }
}