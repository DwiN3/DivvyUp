package com.dwin.du.entity.receipt;

import com.dwin.du.entity.receipt.Request.AddEditReceiptRequest;
import com.dwin.du.entity.receipt.Request.SetSettledRequest;
import com.dwin.du.entity.receipt.Request.SetTotalPriceRequest;
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
    public ResponseEntity<?> addReceipt(@RequestBody AddEditReceiptRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.addReceipt(request, currentUsername);
    }

    @PutMapping("/edit/{id}")
    @Operation(summary = "Edit a receipt", description = "Edits an existing receipt by ID.")
    public ResponseEntity<?> editReceipt(@PathVariable int id, @RequestBody AddEditReceiptRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.editReceipt(id, request, currentUsername);
    }

    @DeleteMapping("/remove/{id}")
    @Operation(summary = "Remove a receipt", description = "Removes a receipt by ID.")
    public ResponseEntity<?> removeReceipt(@PathVariable int id){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.removeReceipt(id, currentUsername);
    }

    @PutMapping("/set-total-price/{id}")
    @Operation(summary = "Set total price of a receipt", description = "Sets the total price for a specific receipt.")
    public ResponseEntity<?> setTotalPrice(@PathVariable int id, @RequestBody SetTotalPriceRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.setTotalPrice(id, request, currentUsername);
    }

    @PutMapping("/set-settled/{id}")
    @Operation(summary = "Set receipt as settled", description = "Marks a receipt as settled by its ID.")
    public ResponseEntity<?> setIsSettled(@PathVariable int id, @RequestBody SetSettledRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.setIsSettled(id, request, currentUsername);
    }

    @GetMapping("/{id}")
    @Operation(summary = "Get a receipt", description = "Retrieves a receipt by ID.")
    public ResponseEntity<?> getReceipt(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.getReceiptById(id, currentUsername);
    }

    @GetMapping("")
    @Operation(summary = "Get all receipts", description = "Retrieves all receipts for the current user.")
    public ResponseEntity<?> getReceipts(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.getReceipts(currentUsername);
    }
}
