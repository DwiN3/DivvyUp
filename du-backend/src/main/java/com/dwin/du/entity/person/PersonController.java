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
    public ResponseEntity<?> add(@RequestBody PersonDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if (authentication == null || !authentication.isAuthenticated())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        String currentUsername = authentication.getName();
        return personService.add(request, currentUsername);
    }

    @PutMapping("/edit/{id}")
    @Operation(summary = "Edit a person", description = "Edits an existing person by ID.")
    public ResponseEntity<?> edit(@PathVariable int id, @RequestBody PersonDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.edit(id, request, currentUsername);
    }

    @DeleteMapping("/remove/{id}")
    @Operation(summary = "Remove a person", description = "Removes a person by ID.")
    public ResponseEntity<?> remove(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.remove(id, currentUsername);
    }

    @PutMapping("/set-receipts-counts/{id}")
    @Operation(summary = "Set receipts counts", description = "Sets the receipts counts for a person by ID.")
    public ResponseEntity<?> setReceiptsCounts(@PathVariable int id, @RequestBody PersonDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.setReceiptsCounts(id, request, currentUsername);
    }

    @PutMapping("/set-total-amount/{id}")
    @Operation(summary = "Set total amount", description = "Sets the total amount for a person by ID.")
    public ResponseEntity<?> setTotaleAmount(@PathVariable int id, @RequestBody PersonDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.setTotalAmount(id, request, currentUsername);
    }

    @GetMapping("/{id}")
    @Operation(summary = "Get person by ID", description = "Retrieves a person by their ID.")
    public ResponseEntity<?> getPerson(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPerson(id, currentUsername);
    }

    @GetMapping("")
    @Operation(summary = "Get all persons", description = "Retrieves all persons.")
    public ResponseEntity<?> getPersons(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPersons(currentUsername);
    }
}
