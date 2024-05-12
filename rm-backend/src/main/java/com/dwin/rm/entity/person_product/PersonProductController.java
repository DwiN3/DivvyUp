package com.dwin.rm.entity.person_product;

import com.dwin.rm.entity.person_product.Request.AddPersonProductRequest;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm")
public class PersonProductController {

    private final PersonProductService personProductService;

    @PostMapping("/product/{productId}/person-product/add")
    public ResponseEntity<?> addPersonProduct(@PathVariable int productId, @RequestBody AddPersonProductRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personProductService.addPersonProduct(productId, request, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/product/{productId}/person-product/show-all")
    public ResponseEntity<?> showPersonProducts(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personProductService.showPersonProducts(productId, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/person-product/show/{personProductId}")
    public ResponseEntity<?> showPersonProductsById(@PathVariable int personProductId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personProductService.showPersonProductsById(personProductId, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @DeleteMapping("/person-product/remove/{personProductId}")
    public ResponseEntity<?> removePersonProduct(@PathVariable int personProductId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personProductService.removePersonProductById(personProductId, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }
}
