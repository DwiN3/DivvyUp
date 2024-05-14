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
        return null;
    }

    @GetMapping("/product/{productId}/person-product/show-all")
    public ResponseEntity<?> showPersonProducts(@PathVariable int productId) {
        return null;
    }

    @GetMapping("/person-product/show/{personProductId}")
    public ResponseEntity<?> showPersonProductsById(@PathVariable int personProductId) {
        return null;
    }

    @DeleteMapping("/person-product/remove/{personProductId}")
    public ResponseEntity<?> removePersonProduct(@PathVariable int personProductId) {
        return null;
    }

}
