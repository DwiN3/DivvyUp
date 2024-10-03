package com.dwin.du.entity.receipt;

import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/receipt")
public class ReceiptController {

    private final ReceiptService receiptService;

    @PostMapping("/add")
    public ResponseEntity<?> addReceipt(@RequestBody ReceiptDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.addReceipt(request, currentUsername);
    }

    @PutMapping("/edit/{receiptId}")
    public ResponseEntity<?> editReceipt(@PathVariable int receiptId, @RequestBody ReceiptDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.editReceipt(receiptId, request, currentUsername);
    }

    @DeleteMapping("/remove/{receiptId}")
    public ResponseEntity<?> removeReceipt(@PathVariable int receiptId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.removeReceipt(receiptId, currentUsername);
    }

    @PutMapping("/set-total-price/{receiptId}")
    public ResponseEntity<?> setTotalAmount(@PathVariable int receiptId, @RequestBody ReceiptDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.setTotalAmount(receiptId, request, currentUsername);
    }

    @PutMapping("/set-settled/{receiptId}")
    public ResponseEntity<?> setIsSettled(@PathVariable int receiptId, @RequestBody ReceiptDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.setIsSettled(receiptId, request, currentUsername);
    }

    @GetMapping("/{receiptId}")
    public ResponseEntity<?> getReceipt(@PathVariable int receiptId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.getReceiptById(receiptId, currentUsername);
    }

    @GetMapping("")
    public ResponseEntity<?> getReceipts(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.getReceipts(currentUsername);
    }
}
