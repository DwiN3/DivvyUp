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

    @PostMapping("/add")
    @Operation(summary = "Add a new person", description = "Adds a new person to the system.")
    public ResponseEntity<?> addPerson(@RequestBody PersonDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if (authentication == null || !authentication.isAuthenticated())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        String currentUsername = authentication.getName();
        return personService.addPerson(request, currentUsername);
    }

    @PutMapping("/edit/{personId}")
    @Operation(summary = "Edit a person", description = "Edits an existing person by personId.")
    public ResponseEntity<?> editPerson(@PathVariable int personId, @RequestBody PersonDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.editPerson(personId, request, currentUsername);
    }

    @DeleteMapping("/remove/{personId}")
    @Operation(summary = "Remove a person", description = "Removes a person by personId.")
    public ResponseEntity<?> removePerson(@PathVariable int personId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.removePerson(personId, currentUsername);
    }

    @PutMapping("/set-receipts-counts/{personId}")
    @Operation(summary = "Set receipts counts", description = "Sets the receipts counts for a person by personId.")
    public ResponseEntity<?> setReceiptsCounts(@PathVariable int personId, @RequestBody PersonDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.setReceiptsCounts(personId, request, currentUsername);
    }

    @PutMapping("/set-total-amount/{personId}")
    @Operation(summary = "Set total amount", description = "Sets the total amount for a person by personId.")
    public ResponseEntity<?> setTotaleAmount(@PathVariable int personId, @RequestBody PersonDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.setTotalAmount(personId, request, currentUsername);
    }

    @GetMapping("/{personId}")
    @Operation(summary = "Get person by ID", description = "Retrieves a person by their ID.")
    public ResponseEntity<?> getPerson(@PathVariable int personId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPersonById(personId, currentUsername);
    }

    @GetMapping("")
    @Operation(summary = "Get all persons", description = "Retrieves all persons.")
    public ResponseEntity<?> getPersons(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPersons(currentUsername);
    }
}
