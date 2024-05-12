package com.dwin.rm.entity.receipt;

import com.dwin.rm.entity.receipt.Request.AddReceiptRequest;
import com.dwin.rm.security.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class ReceiptService {

    private final ReceiptRepository receiptRepository;
    private final UserRepository userRepository;

    public ResponseEntity<?> addReceipt(AddReceiptRequest request, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var receipt = Receipt.builder()
                .addedByUserId(user.getUserId())
                .receiptName(request.getReceiptName())
                .date(request.getDate())
                .build();

        receiptRepository.save(receipt);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> editReceipt(int receiptId, AddReceiptRequest request, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var existingReceipt = receiptRepository.findById(receiptId).orElse(null);
        if (existingReceipt == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
        if (existingReceipt.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }

        existingReceipt.setReceiptName(request.getReceiptName());
        existingReceipt.setDate(request.getDate());
        receiptRepository.save(existingReceipt);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removeReceipt(int receiptId, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var receipt = receiptRepository.findById(receiptId).orElse(null);
        if (receipt == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
        if (receipt.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }
        receiptRepository.delete(receipt);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> showReceiptById(int receiptId, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var receipt = receiptRepository.findById(receiptId).orElse(null);
        if (receipt == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
        if (receipt.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }
        return ResponseEntity.ok(receipt);
    }

    public ResponseEntity<?> showReceipts(String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var receipts = receiptRepository.findAllByAddedByUserId(user.getUserId());
        return ResponseEntity.ok(receipts);
    }

    public ResponseEntity<?> setTotalAmount(int receiptID, double totalAmount, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var receipt = receiptRepository.findById(receiptID).orElse(null);
        if (receipt == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
        if (receipt.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }
        receipt.setTotalAmount(totalAmount);
        receiptRepository.save(receipt);
        return ResponseEntity.ok().build();
    }
}
