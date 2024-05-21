package com.dwin.rm.entity.product;


import com.dwin.rm.entity.product.Request.AddProductRequest;
import com.dwin.rm.entity.receipt.Request.SetIsSettledRequest;
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
        return null;
    }

    @PutMapping("/product/edit/{productId}")
    public ResponseEntity<?> editProduct(@PathVariable int productId, @RequestBody AddProductRequest request) {
        return null;
    }

    @DeleteMapping("/product/remove/{productId}")
    public ResponseEntity<?> removeProduct(@PathVariable int productId) {
        return null;
    }

    @PutMapping("/product/set-is-settled/{productId}")
    public ResponseEntity<?> setIsSettled(@PathVariable int productId, @RequestBody SetIsSettledRequest request){
        return null;
    }

    @GetMapping("/product/show/{productId}")
    public ResponseEntity<?> showProduct(@PathVariable int productId) {
        return null;
    }

    @GetMapping("/receipt/{receiptID}/product/show-all")
    public ResponseEntity<?> showProducts(@PathVariable int receiptID) {
        return null;
    }
}