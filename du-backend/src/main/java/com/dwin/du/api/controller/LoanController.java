package com.dwin.du.api.controller;
import com.dwin.du.api.request.AddEditLoanRequest;
import com.dwin.du.api.service.LoanService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/loan")
@Tag(name = "Loan Management", description = "APIs for managing loans")
public class LoanController {

    private final LoanService loanService;

    @PostMapping("/add")
    @Operation(summary = "Add a new loan", description = "Adds a new loan to the system.")
    public ResponseEntity<?> addLoan(@RequestBody AddEditLoanRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return loanService.addLoan(username, request);
    }

    @PutMapping("/{loanId}/edit")
    @Operation(summary = "Edit a loan", description = "Edits the details of an existing loan by its ID.")
    public ResponseEntity<?> editLoan(@PathVariable int loanId, @RequestBody AddEditLoanRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return loanService.editLoan(username, loanId, request);
    }

    @DeleteMapping("/{loanId}/remove")
    @Operation(summary = "Remove a loan", description = "Removes a loan from the system by its ID.")
    public ResponseEntity<?> removeLoan(@PathVariable int loanId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return loanService.removeLoan(username, loanId);
    }

    @PutMapping("/{loanId}/set-settled")
    @Operation(summary = "Set loan as settled", description = "Marks a loan as settled by its ID.")
    public ResponseEntity<?> setIsSettled(@PathVariable int loanId, @RequestParam boolean settled) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return loanService.setIsSettled(username, loanId, settled);
    }

    @PutMapping("/{loanId}/set-lent")
    @Operation(summary = "Set loan as lent", description = "Marks a loan as lent by its ID.")
    public ResponseEntity<?> setIsLent(@PathVariable int loanId, @RequestParam boolean lent) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return loanService.setIsLent(username, loanId, lent);
    }

    @GetMapping("/{loanId}")
    @Operation(summary = "Retrieve a loan", description = "Retrieves the details of a loan by its ID.")
    public ResponseEntity<?> getLoan(@PathVariable int loanId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return loanService.getLoan(username, loanId);
    }

    @GetMapping("/person/{personId}")
    @Operation(summary = "Retrieve a loan by person", description = "Retrieves the details of a loan associated with a specific person by their ID.")
    public ResponseEntity<?> getLoanPerson(@PathVariable int personId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return loanService.getLoanPerson(username, personId);
    }

    @GetMapping("")
    @Operation(summary = "Retrieve all loans", description = "Retrieves all loans associated with the current user.")
    public ResponseEntity<?> getLoans(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return loanService.getLoans(username);
    }
}
