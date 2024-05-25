package com.dwin.du.entity.product;


import com.dwin.du.entity.product.Request.AddProductRequest;
import com.dwin.du.entity.receipt.Request.SetIsSettledRequest;
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
        return productService.addProductToReceipt(request, receiptID, currentUsername);
    }

    @DeleteMapping("/product/remove/{productId}")
    public ResponseEntity<?> removeProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.removeProduct(productId, currentUsername);
    }

    @PutMapping("/product/set-is-settled/{productId}")
    public ResponseEntity<?> setIsSettled(@PathVariable int productId, @RequestBody SetIsSettledRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.setIsSettled(productId, request, currentUsername);
    }

    @GetMapping("/product/show/{productId}")
    public ResponseEntity<?> showProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.showProductById(productId, currentUsername);
    }

    @GetMapping("/receipt/{receiptID}/product/show-all")
    public ResponseEntity<?> showProducts(@PathVariable int receiptID) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return productService.showAllProductsFromReceipt(receiptID, currentUsername);
    }
}