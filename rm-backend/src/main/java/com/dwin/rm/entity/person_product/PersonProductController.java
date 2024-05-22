package com.dwin.rm.entity.person_product;

import com.dwin.rm.entity.person_product.Request.AddPersonProductRequest;
import com.dwin.rm.entity.receipt.Request.SetIsSettledRequest;
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
        return personProductService.addPersonProduct(request, productId, currentUsername);
    }

    @DeleteMapping("/person-product/remove/{personProductId}")
    public ResponseEntity<?> removePersonProduct(@PathVariable int personProductId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.removePersonProduct(personProductId, currentUsername);
    }

    @PutMapping("/person-product/set-is-settled/{personProductId}")
    public ResponseEntity<?> setIsSettled(@PathVariable int personProductId, @RequestBody SetIsSettledRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.setIsSettled(personProductId, request, currentUsername);
    }

    @PutMapping("/person-product/set-is-compensation/{personProductId}")
    public ResponseEntity<?> setIsCompensation(@PathVariable int personProductId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.setIsCompensation(personProductId, currentUsername);
    }

    @GetMapping("/person-product/show/{personProductId}")
    public ResponseEntity<?> showPersonProduct(@PathVariable int personProductId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.showPersonProduct(personProductId, currentUsername);
    }

    @GetMapping("/product/{productId}/person-product/show-all")
    public ResponseEntity<?> showPersonProducts(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.showPersonProducts(productId, currentUsername);
    }
}
