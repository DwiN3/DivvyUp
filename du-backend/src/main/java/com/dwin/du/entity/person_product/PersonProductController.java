package com.dwin.du.entity.person_product;

import com.dwin.du.entity.person_product.Request.AddPersonProductRequest;
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

    @PostMapping("/product/{productId}/person-product/add")
    @Operation(summary = "Add person to product", description = "Associates a person with a specific product.")
    public ResponseEntity<?> addPersonProduct(@PathVariable int productId, @RequestBody AddPersonProductRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.addPersonProduct(request, productId, currentUsername);
    }

    @DeleteMapping("/person-product/{id}/remove")
    @Operation(summary = "Remove person-product association", description = "Removes an association between a person and a product.")
    public ResponseEntity<?> removePersonProduct(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.removePersonProduct(id, currentUsername);
    }

    @PutMapping("/person-product/{id}/change-person")
    @Operation(summary = "Set person in person-product", description = "Set person in person-product.")
    public ResponseEntity<?> changePerson(@PathVariable int id, @RequestParam int personId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.changePerson(id, personId, currentUsername);
    }

    @PutMapping("/person-product/{id}/set-settled")
    @Operation(summary = "Set person-product as settled", description = "Marks a person-product association as settled.")
    public ResponseEntity<?> setIsSettled(@PathVariable int id, @RequestParam boolean settled){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.setIsSettled(id, settled, currentUsername);
    }

    @PutMapping("/person-product/{id}/set-compensation")
    @Operation(summary = "Set person-product as compensation", description = "Marks a person-product association as a compensation.")
    public ResponseEntity<?> setIsCompensation(@PathVariable int id){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.setIsCompensation(id, currentUsername);
    }

    @GetMapping("/person-product/{id}")
    @Operation(summary = "Get person-product association", description = "Retrieves a person-product association by ID.")
    public ResponseEntity<?> getPersonProduct(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.getPersonProductById(id, currentUsername);
    }

    @GetMapping("/product/{productId}/person-product")
    @Operation(summary = "Get all person-product associations for a product", description = "Retrieves all person-product associations for a specific product.")
    public ResponseEntity<?> getPersonProducts(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.getProductPersonProductsFromProduct(productId, currentUsername);
    }

    @GetMapping("/person-product")
    @Operation(summary = "Get all person-product for user", description = "Retrieves all person-product for user.")
    public ResponseEntity<?> getAllPersonProducts() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.getAllProductPersonProductsFromProduct(currentUsername);
    }
}
