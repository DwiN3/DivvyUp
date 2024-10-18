package com.dwin.du.entity.person_product;

import com.dwin.du.entity.person_product.Request.AddEditPersonProductRequest;
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
@Tag(name = "Person Product Management", description = "APIs for managing person-product relationships.")
public class PersonProductController {

    private final PersonProductService personProductService;

    @PostMapping("/product/{productId}/person-product/add")
    @Operation(summary = "Add person to product", description = "Associates a person with a specific product.")
    public ResponseEntity<?> addPersonProduct(@PathVariable int productId, @RequestBody AddEditPersonProductRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personProductService.addPersonProduct(username, productId, request);
    }

    @PutMapping("/person-product/{personProductId}/edit")
    @Operation(summary = "Edit person-product association", description = "Edits the details of an existing person-product association by its ID.")
    public ResponseEntity<?> editPersonProduct(@PathVariable int personProductId, @RequestBody AddEditPersonProductRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personProductService.editPersonProduct(username, personProductId, request);
    }

    @DeleteMapping("/person-product/{personProductId}/remove")
    @Operation(summary = "Remove person-product association", description = "Removes an association between a person and a product by its ID.")
    public ResponseEntity<?> removePersonProduct(@PathVariable int personProductId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personProductService.removePersonProduct(username, personProductId);
    }

    @PutMapping("/person-product/{personProductId}/set-person")
    @Operation(summary = "Set person in person-product", description = "Changes the person in an existing person-product association.")
    public ResponseEntity<?> setPerson(@PathVariable int personProductId, @RequestParam int personId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personProductService.setPerson(username, personProductId, personId);
    }

    @PutMapping("/person-product/{personProductId}/set-settled")
    @Operation(summary = "Set person-product as settled", description = "Marks a person-product association as settled.")
    public ResponseEntity<?> setIsSettled(@PathVariable int personProductId, @RequestParam boolean settled){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personProductService.setIsSettled(username, personProductId, settled);
    }

    @PutMapping("/person-product/{personProductId}/set-compensation")
    @Operation(summary = "Set person-product as compensation", description = "Marks a person-product association as compensation.")
    public ResponseEntity<?> setIsCompensation(@PathVariable int personProductId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personProductService.setIsCompensation(username, personProductId);
    }

    @GetMapping("/person-product/{personProductId}")
    @Operation(summary = "Retrieve person-product association", description = "Retrieves the details of a person-product association by its ID.")
    public ResponseEntity<?> getPersonProduct(@PathVariable int personProductId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personProductService.getPersonProduct(username, personProductId);
    }

    @GetMapping("/product/{productId}/person-product")
    @Operation(summary = "Retrieve all person-product associations for a product", description = "Retrieves all person-product associations related to a specific product.")
    public ResponseEntity<?> getPersonProductsFromProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personProductService.getPersonProductsFromProduct(username, productId);
    }

    @GetMapping("/person-product")
    @Operation(summary = "Retrieve all person-product associations for user", description = "Retrieves all person-product associations related to the logged-in user.")
    public ResponseEntity<?> getAllPersonProducts() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personProductService.getAllPersonProducts(username);
    }
}
