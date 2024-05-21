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
        return null;
    }

    @PutMapping("/edit/{receiptId}")
    public ResponseEntity<?> editReceipt(@PathVariable int receiptId, @RequestBody AddReceiptRequest request) {
        return null;
    }

    @DeleteMapping("/remove/{receiptId}")
    public ResponseEntity<?> removeReceipt(@PathVariable int receiptId){
        return null;
    }

    @PutMapping("/set-total-amount/{receiptId}")
    public ResponseEntity<?> setTotalAmount(@PathVariable int receiptId, @RequestBody SetTotalAmountReceiptRequest request){
        return null;
    }

    @PutMapping("/set-is-settled/{receiptId}")
    public ResponseEntity<?> setIsSettled(@PathVariable int receiptId, @RequestBody SetIsSettledRequest request){
        return null;
    }

    @GetMapping("/show/{receiptId}")
    public ResponseEntity<?> showReceipt(@PathVariable int receiptId) {
        return null;
    }

    @GetMapping("/show-all")
    public ResponseEntity<?> showReceipts(){
        return null;
    }
}
