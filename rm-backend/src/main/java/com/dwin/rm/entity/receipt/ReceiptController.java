package com.dwin.rm.entity.receipt;

import com.dwin.rm.entity.receipt.Request.AddReceiptRequest;
import com.dwin.rm.entity.receipt.Request.SetIsSettledRequest;
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

    private final ReceiptService receiptService;

    @PostMapping("/add")
    public ResponseEntity<?> addReceipt(@RequestBody AddReceiptRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.addReceipt(request, currentUsername);
    }

    @PutMapping("/edit/{receiptId}")
    public ResponseEntity<?> editReceipt(@PathVariable int receiptId, @RequestBody AddReceiptRequest request) {
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

    @PutMapping("/set-total-amount/{receiptId}")
    public ResponseEntity<?> setTotalAmount(@PathVariable int receiptId, @RequestBody SetTotalAmountReceiptRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.setTotalAmount(receiptId, request, currentUsername);
    }

    @PutMapping("/set-is-settled/{receiptId}")
    public ResponseEntity<?> setIsSettled(@PathVariable int receiptId, @RequestBody SetIsSettledRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.setIsSettled(receiptId, request, currentUsername);
    }

    @GetMapping("/show/{receiptId}")
    public ResponseEntity<?> showReceipt(@PathVariable int receiptId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.showReceipt(receiptId, currentUsername);
    }

    @GetMapping("/show-all")
    public ResponseEntity<?> showReceipts(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return receiptService.showReceipts(currentUsername);
    }
}
