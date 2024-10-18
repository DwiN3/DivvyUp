package com.dwin.du.api.controller;
import com.dwin.du.api.request.AddEditPersonRequest;
import com.dwin.du.api.service.PersonService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
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
        String username = authentication.getName();
        return personService.addPerson(username, request);
    }

    @PutMapping("/{personId}/edit")
    @Operation(summary = "Edit a person", description = "Edits the details of an existing person by their ID.")
    public ResponseEntity<?> editPerson(@PathVariable int personId, @RequestBody AddEditPersonRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.editPerson(username, personId, request);
    }

    @DeleteMapping("/{personId}/remove")
    @Operation(summary = "Remove a person", description = "Removes a person from the system using their ID.")
    public ResponseEntity<?> removePerson(@PathVariable int personId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.removePerson(username, personId);
    }

    @PutMapping("/{personId}/set-receipts-counts")
    @Operation(summary = "Set receipts counts", description = "Updates the number of receipts associated with a person by their ID.")
    public ResponseEntity<?> setReceiptsCounts(@PathVariable int personId, @RequestParam int receiptsCount) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.setReceiptsCounts(username, personId, receiptsCount);
    }

    @PutMapping("/{personId}/set-total-amount")
    @Operation(summary = "Set total amount", description = "Sets the total amount associated with a person by their ID.")
    public ResponseEntity<?> setTotalAmount(@PathVariable int personId, @RequestParam Double totalAmount) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.setTotalAmount(username, personId, totalAmount);
    }

    @GetMapping("/{personId}")
    @Operation(summary = "Retrieve person by ID", description = "Retrieves the details of a person by their ID.")
    public ResponseEntity<?> getPerson(@PathVariable int personId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.getPerson(username, personId);
    }

    @GetMapping("")
    @Operation(summary = "Retrieve all persons", description = "Retrieves all persons associated with the user's account.")
    public ResponseEntity<?> getPersons(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.getPersons(username);
    }


    @GetMapping("/user-person")
    @Operation(summary = "Retrieve person of logged-in user", description = "Retrieves the person entity representing the currently logged-in user.")
    public ResponseEntity<?> getUserPerson() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.getUserPerson(username);
    }

    @GetMapping("/{receiptId}/from-receipt")
    @Operation(summary = "Retrieve all persons from receipt", description = "Retrieves all persons associated with a specific receipt by its ID.")
    public ResponseEntity<?> getPersonsFromReceipt(@PathVariable int receiptId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.getPersonsFromReceipt(username, receiptId);
    }

    @GetMapping("/{productId}/from-product")
    @Operation(summary = "Retrieve all persons from product", description = "Retrieves all persons associated with a specific product by its ID.")
    public ResponseEntity<?> getPersonsFromProduct(@PathVariable int productId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.getPersonsFromProduct(username, productId);
    }
}
