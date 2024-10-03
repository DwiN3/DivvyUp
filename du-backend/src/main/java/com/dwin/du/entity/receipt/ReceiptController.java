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

    @Operation(summary = "Add a new receipt", description = "Adds a new receipt.")
    @PostMapping("/add")
    public ResponseEntity<?> addReceipt(@RequestBody ReceiptDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.addReceipt(request, currentUsername);
    }

    @Operation(summary = "Edit a receipt", description = "Edits an existing receipt by ID.")
    @PutMapping("/edit/{receiptId}")
    public ResponseEntity<?> editReceipt(@PathVariable int receiptId, @RequestBody ReceiptDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.editReceipt(receiptId, request, currentUsername);
    }

    @Operation(summary = "Remove a receipt", description = "Removes a receipt by ID.")
    @DeleteMapping("/remove/{receiptId}")
    public ResponseEntity<?> removeReceipt(@PathVariable int receiptId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.removeReceipt(receiptId, currentUsername);
    }

    @Operation(summary = "Set total price of a receipt", description = "Sets the total price for a specific receipt.")
    @PutMapping("/set-total-price/{receiptId}")
    public ResponseEntity<?> setTotalAmount(@PathVariable int receiptId, @RequestBody ReceiptDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.setTotalAmount(receiptId, request, currentUsername);
    }

    @Operation(summary = "Set receipt as settled", description = "Marks a receipt as settled by its ID.")
    @PutMapping("/set-settled/{receiptId}")
    public ResponseEntity<?> setIsSettled(@PathVariable int receiptId, @RequestBody ReceiptDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.setIsSettled(receiptId, request, currentUsername);
    }

    @Operation(summary = "Get a receipt", description = "Retrieves a receipt by ID.")
    @GetMapping("/{receiptId}")
    public ResponseEntity<?> getReceipt(@PathVariable int receiptId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.getReceiptById(receiptId, currentUsername);
    }

    @Operation(summary = "Get all receipts", description = "Retrieves all receipts for the current user.")
    @GetMapping("")
    public ResponseEntity<?> getReceipts(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.getReceipts(currentUsername);
    }
}
