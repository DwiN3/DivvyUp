package com.dwin.du.api.controller;
import com.dwin.du.api.request.AddEditReceiptRequest;
import com.dwin.du.api.service.ReceiptService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;

import java.util.Date;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/receipt")
@Tag(name = "Receipt Management", description = "APIs for managing receipts")
public class ReceiptController {

    private final ReceiptService receiptService;

    @PostMapping("/add")
    @Operation(summary = "Add a new receipt", description = "Adds a new receipt to the system.")
    public ResponseEntity<?> addReceipt(@RequestBody AddEditReceiptRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return receiptService.addReceipt(username, request);
    }

    @PutMapping("/{receiptId}/edit")
    @Operation(summary = "Edit a receipt", description = "Edits the details of an existing receipt by its ID..")
    public ResponseEntity<?> editReceipt(@PathVariable int receiptId, @RequestBody AddEditReceiptRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return receiptService.editReceipt(username, receiptId, request);
    }

    @DeleteMapping("/{receiptId}/remove")
    @Operation(summary = "Remove a receipt", description = "Removes a receipt from the system by its ID.")
    public ResponseEntity<?> removeReceipt(@PathVariable int receiptId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return receiptService.removeReceipt(username, receiptId);
    }

    @PutMapping("/{receiptId}/set-settled")
    @Operation(summary = "Set receipt as settled", description = "Marks a receipt as settled by its ID.")
    public ResponseEntity<?> setIsSettled(@PathVariable int receiptId, @RequestParam boolean settled) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return receiptService.setIsSettled(username, receiptId, settled);
    }

    @GetMapping("/{receiptId}")
    @Operation(summary = "Retrieve a receipt", description = "Retrieves the details of a receipt by its ID.")
    public ResponseEntity<?> getReceipt(@PathVariable int receiptId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return receiptService.getReceipt(username, receiptId);
    }

    @GetMapping("")
    @Operation(summary = "Retrieve all receipts", description = "Retrieves all receipts associated with the current user.")
    public ResponseEntity<?> getReceipts(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return receiptService.getReceipts(username);
    }

    @GetMapping("/date-range")
    @Operation(summary = "Retrieve all receipts in date range", description = "Retrieves all receipts in date range associated with the current user.")
    public ResponseEntity<?> getReceiptsByDataRange(@RequestParam("from") String fromDate, @RequestParam("to") String toDate) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return receiptService.getReceiptsByDataRange(username, fromDate, toDate);
    }
}
