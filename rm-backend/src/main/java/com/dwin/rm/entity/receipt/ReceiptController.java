package com.dwin.rm.entity.receipt;

import com.dwin.rm.entity.person.Request.AddPersonRequest;
import com.dwin.rm.entity.person.Request.SetPersonReceiptsCountsRequest;
import com.dwin.rm.entity.receipt.Request.AddReceiptRequest;
import com.dwin.rm.entity.receipt.Request.SetTotalAmountReceiptRequest;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/receipt")
public class ReceiptController {
/*
    private final ReceiptService service;

    @PostMapping("/add")
    public ResponseEntity<?> addReceipt(@RequestBody AddReceiptRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = service.addReceipt(request, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PutMapping("/edit/{receiptId}")
    public ResponseEntity<?> editReceipt(@PathVariable int receiptId, @RequestBody AddReceiptRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = service.editReceipt(receiptId, request, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @DeleteMapping("/remove/{receiptId}")
    public ResponseEntity<?> removeReceipt(@PathVariable int receiptId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = service.removeReceipt(receiptId, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/show/{receiptId}")
    public ResponseEntity<?> showReceiptById(@PathVariable int receiptId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = service.showReceiptById(receiptId, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/show-all")
    public ResponseEntity<?> showReceipts(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = service.showReceipts(currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PutMapping("/set-total-amount/{receiptId}")
    public ResponseEntity<?> setTotalAmount(@PathVariable int receiptId, @RequestBody SetTotalAmountReceiptRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = service.setTotalAmount(receiptId, request.getTotalAmount(), currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }*/
}
