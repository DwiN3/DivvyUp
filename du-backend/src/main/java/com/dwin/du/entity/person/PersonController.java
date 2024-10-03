package com.dwin.du.entity.person;

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

    @Operation(summary = "Add a new person", description = "Adds a new person to the system.")
    @PostMapping("/add")
    public ResponseEntity<?> addPerson(@RequestBody PersonDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if (authentication == null || !authentication.isAuthenticated())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        String currentUsername = authentication.getName();
        return personService.addPerson(request, currentUsername);
    }

    @Operation(summary = "Edit a person", description = "Edits an existing person by personId.")
    @PutMapping("/edit/{personId}")
    public ResponseEntity<?> editPerson(@PathVariable int personId, @RequestBody PersonDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.editPerson(personId, request, currentUsername);
    }

    @Operation(summary = "Remove a person", description = "Removes a person by personId.")
    @DeleteMapping("/remove/{personId}")
    public ResponseEntity<?> removePerson(@PathVariable int personId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.removePerson(personId, currentUsername);
    }

    @Operation(summary = "Set receipts counts", description = "Sets the receipts counts for a person by personId.")
    @PutMapping("/set-receipts-counts/{personId}")
    public ResponseEntity<?> setReceiptsCounts(@PathVariable int personId, @RequestBody PersonDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.setReceiptsCounts(personId, request, currentUsername);
    }

    @Operation(summary = "Set total amount", description = "Sets the total amount for a person by personId.")
    @PutMapping("/set-total-amount/{personId}")
    public ResponseEntity<?> setTotaleAmount(@PathVariable int personId, @RequestBody PersonDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.setTotalAmount(personId, request, currentUsername);
    }

    @Operation(summary = "Get person by ID", description = "Retrieves a person by their ID.")
    @GetMapping("/{personId}")
    public ResponseEntity<?> getPerson(@PathVariable int personId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPersonById(personId, currentUsername);
    }

    @Operation(summary = "Get all persons", description = "Retrieves all persons.")
    @GetMapping("")
    public ResponseEntity<?> getPersons(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPersons(currentUsername);
    }
}
