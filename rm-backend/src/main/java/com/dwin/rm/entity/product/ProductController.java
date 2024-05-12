package com.dwin.rm.entity.product;


import com.dwin.rm.entity.product.Request.AddProductRequest;
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
    public ResponseEntity<?> addProductToReceipt(@PathVariable int receiptID, @RequestBody AddProductRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = productService.addProductToReceipt(receiptID, request, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PutMapping("/product/edit/{productId}")
    public ResponseEntity<?> editProduct(@PathVariable int productId, @RequestBody AddProductRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = productService.editProduct(productId, request, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @DeleteMapping("/product/remove/{productId}")
    public ResponseEntity<?> removeProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = productService.removeProduct(productId, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/product/show/{productId}")
    public ResponseEntity<?> showProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = productService.showProduct(productId, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/receipt/{receiptID}/product/show-all")
    public ResponseEntity<?> showAllProductsFromReceipt(@PathVariable int receiptID) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = productService.showAllProductsFromReceipt(receiptID, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }
}
