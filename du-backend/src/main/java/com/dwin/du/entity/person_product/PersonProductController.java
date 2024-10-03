package com.dwin.du.entity.person_product;

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
@Tag(name = "Person Product Management", description = "APIs for managing person-product relationships")
public class PersonProductController {

    private final PersonProductService personProductService;

    @Operation(summary = "Add person to product", description = "Associates a person with a specific product.")
    @PostMapping("/product/{productId}/person-product/add")
    public ResponseEntity<?> addPersonProduct(@PathVariable int productId, @RequestBody PersonProductDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.addPersonProduct(request, productId, currentUsername);
    }

    @Operation(summary = "Remove person-product association", description = "Removes an association between a person and a product.")
    @DeleteMapping("/person-product/remove/{personProductId}")
    public ResponseEntity<?> removePersonProduct(@PathVariable int personProductId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.removePersonProduct(personProductId, currentUsername);
    }

    @Operation(summary = "Set person-product as settled", description = "Marks a person-product association as settled.")
    @PutMapping("/person-product/set-settled/{personProductId}")
    public ResponseEntity<?> setIsSettled(@PathVariable int personProductId, @RequestBody PersonProductDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.setIsSettled(personProductId, request, currentUsername);
    }

    @Operation(summary = "Set person-product as compensation", description = "Marks a person-product association as a compensation.")
    @PutMapping("/person-product/set-compensation/{personProductId}")
    public ResponseEntity<?> setIsCompensation(@PathVariable int personProductId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.setIsCompensation(personProductId, currentUsername);
    }

    @Operation(summary = "Get person-product association", description = "Retrieves a person-product association by ID.")
    @GetMapping("/person-product/{personProductId}")
    public ResponseEntity<?> getPersonProduct(@PathVariable int personProductId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.getPersonProductById(personProductId, currentUsername);
    }

    @Operation(summary = "Get all person-product associations for a product", description = "Retrieves all person-product associations for a specific product.")
    @GetMapping("/product/{productId}/person-product")
    public ResponseEntity<?> getPersonProducts(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.getProductPersonProductsFromProduct(productId, currentUsername);
    }
}
