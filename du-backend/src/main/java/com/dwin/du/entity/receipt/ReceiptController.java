package com.dwin.du.entity.receipt;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/receipt")
@Tag(name = "Receipt Management", description = "APIs for managing receipts")
public class ReceiptController {

    private final ReceiptService receiptService;

    @PostMapping("/add")
    @Operation(summary = "Add a new receipt", description = "Adds a new receipt.")
    public ResponseEntity<?> addReceipt(@RequestBody ReceiptDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.add(request, currentUsername);
    }

    @PutMapping("/edit/{id}")
    @Operation(summary = "Edit a receipt", description = "Edits an existing receipt by ID.")
    public ResponseEntity<?> edit(@PathVariable int id, @RequestBody ReceiptDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.edit(id, request, currentUsername);
    }

    @DeleteMapping("/remove/{id}")
    @Operation(summary = "Remove a receipt", description = "Removes a receipt by ID.")
    public ResponseEntity<?> remove(@PathVariable int id){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.remove(id, currentUsername);
    }

    @PutMapping("/set-total-price/{id}")
    @Operation(summary = "Set total price of a receipt", description = "Sets the total price for a specific receipt.")
    public ResponseEntity<?> setTotalAmount(@PathVariable int id, @RequestBody ReceiptDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.setTotalAmount(id, request, currentUsername);
    }

    @PutMapping("/set-settled/{id}")
    @Operation(summary = "Set receipt as settled", description = "Marks a receipt as settled by its ID.")
    public ResponseEntity<?> setIsSettled(@PathVariable int id, @RequestBody ReceiptDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.setIsSettled(id, request, currentUsername);
    }

    @GetMapping("/{id}")
    @Operation(summary = "Get a receipt", description = "Retrieves a receipt by ID.")
    public ResponseEntity<?> getReceipt(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.getReceipt(id, currentUsername);
    }

    @GetMapping("")
    @Operation(summary = "Get all receipts", description = "Retrieves all receipts for the current user.")
    public ResponseEntity<?> getReceipts(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.getReceipts(currentUsername);
    }
}
