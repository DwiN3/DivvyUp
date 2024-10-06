package com.dwin.du.entity.person;

import com.dwin.du.entity.person.Request.AddEditPersonRequest;
import com.dwin.du.entity.person.Request.SetReceiptsCountsRequest;
import com.dwin.du.entity.person.Request.SetTotalAmountRequest;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;
import lombok.RequiredArgsConstructor;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/person")
@Tag(name = "Person Management", description = "APIs for managing persons")
public class PersonController {

  private final PersonService personService;

    @PostMapping("/add")
    @Operation(summary = "Add a new person", description = "Adds a new person to the system.")
    public ResponseEntity<?> addPerson(@RequestBody AddEditPersonRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if (authentication == null || !authentication.isAuthenticated())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        String currentUsername = authentication.getName();
        return personService.addPerson(request, currentUsername);
    }

    @PutMapping("/{id}/edit")
    @Operation(summary = "Edit a person", description = "Edits an existing person by personId.")
    public ResponseEntity<?> editPerson(@PathVariable int id, @RequestBody AddEditPersonRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.editPerson(id, request, currentUsername);
    }

    @DeleteMapping("/{id}/remove")
    @Operation(summary = "Remove a person", description = "Removes a person by personId.")
    public ResponseEntity<?> removePerson(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.removePerson(id, currentUsername);
    }

    @PutMapping("/{id}/set-receipts-counts")
    @Operation(summary = "Set receipts counts", description = "Sets the receipts counts for a person by personId.")
    public ResponseEntity<?> setReceiptsCounts(@PathVariable int id, @RequestBody SetReceiptsCountsRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.setReceiptsCounts(id, request, currentUsername);
    }

    @PutMapping("/{id}/set-total-amount")
    @Operation(summary = "Set total amount", description = "Sets the total amount for a person by personId.")
    public ResponseEntity<?> setTotaleAmount(@PathVariable int id, @RequestBody SetTotalAmountRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.setTotalAmount(id, request, currentUsername);
    }

    @GetMapping("/{id}")
    @Operation(summary = "Get person by ID", description = "Retrieves a person by their ID.")
    public ResponseEntity<?> getPerson(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPersonById(id, currentUsername);
    }

    @GetMapping("")
    @Operation(summary = "Get all persons", description = "Retrieves all persons.")
    public ResponseEntity<?> getPersons(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPersons(currentUsername);
    }

    @GetMapping("/{receiptId}/from-receipt")
    @Operation(summary = "Get all persons from receipt", description = "Retrieves all persons associated with a specific receipt.")
    public ResponseEntity<?> getPersonsReceipt(@PathVariable int receiptId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPersonsFromReceipt(currentUsername, receiptId);
    }

    @GetMapping("/{productId}/from-product")
    @Operation(summary = "Get all persons from product", description = "Retrieves all persons associated with a specific product.")
    public ResponseEntity<?> getPersonsProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPersonsFromProduct(currentUsername, productId);
    }

}
