package com.dwin.du.entity.person_product;

import com.dwin.du.entity.person_product.Request.AddPersonProductRequest;
import com.dwin.du.entity.product.Request.AddProductRequest;
import com.dwin.du.entity.receipt.Request.SetSettledRequest;
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

    @DeleteMapping("/person-product/remove/{personProductId}")
    @Operation(summary = "Remove person-product association", description = "Removes an association between a person and a product.")
    public ResponseEntity<?> removePersonProduct(@PathVariable int personProductId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.removePersonProduct(personProductId, currentUsername);
    }

    @PutMapping("/person-product/set-settled/{personProductId}")
    @Operation(summary = "Set person-product as settled", description = "Marks a person-product association as settled.")
    public ResponseEntity<?> setIsSettled(@PathVariable int personProductId, @RequestBody SetSettledRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.setIsSettled(personProductId, request, currentUsername);
    }

    @PutMapping("/person-product/set-compensation/{personProductId}")
    @Operation(summary = "Set person-product as compensation", description = "Marks a person-product association as a compensation.")
    public ResponseEntity<?> setIsCompensation(@PathVariable int personProductId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.setIsCompensation(personProductId, currentUsername);
    }

    @GetMapping("/person-product/{personProductId}")
    @Operation(summary = "Get person-product association", description = "Retrieves a person-product association by ID.")
    public ResponseEntity<?> getPersonProduct(@PathVariable int personProductId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.getPersonProductById(personProductId, currentUsername);
    }

    @GetMapping("/product/{productId}/person-product")
    @Operation(summary = "Get all person-product associations for a product", description = "Retrieves all person-product associations for a specific product.")
    public ResponseEntity<?> getPersonProducts(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personProductService.getProductPersonProductsFromProduct(productId, currentUsername);
    }
}
