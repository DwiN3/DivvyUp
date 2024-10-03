package com.dwin.du.entity.receipt;

import com.dwin.du.entity.user.User;
import com.dwin.du.entity.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.util.*;

@Service
@RequiredArgsConstructor
public class ReceiptService {

    private final UserRepository userRepository;
    private final ReceiptRepository receiptRepository;

    public ResponseEntity<?> addReceipt(ReceiptDto request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

            User user = userRepository.findByUsername(username).get();
            Receipt receipt = Receipt.builder()
                    .user(user)
                    .receiptName(request.getReceiptName())
                    .date(request.getDate())
                    .totalAmount(0.0)
                    .isSettled(false)
                    .build();
            receiptRepository.save(receipt);
            return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> editReceipt(int receiptId, ReceiptDto request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        receipt.setReceiptName(request.getReceiptName());
        receipt.setDate(request.getDate());
        receiptRepository.save(receipt);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removeReceipt(int receiptId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        receiptRepository.delete(receipt);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setTotalAmount(int receiptId, ReceiptDto request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        receipt.setTotalAmount(request.getTotalAmount());
        receiptRepository.save(receipt);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsSettled(int receiptId, ReceiptDto request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        receipt.setSettled(request.isSettled());
        receiptRepository.save(receipt);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getReceiptById(int receiptId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Receipt response = Receipt.builder()
                .id(receipt.getId())
                .receiptName(receipt.getReceiptName())
                .date(receipt.getDate())
                .totalAmount(receipt.getTotalAmount())
                .isSettled(receipt.isSettled())
                .user(receipt.getUser())
                .build();
        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getReceipts(String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        User user = optionalUser.get();
        List<Receipt> receipts = receiptRepository.findByUser(user);
        List<ReceiptDto> responseList = new ArrayList<>();
        for (Receipt receipt : receipts) {
            ReceiptDto response = ReceiptDto.builder()
                    .id(receipt.getId())
                    .receiptName(receipt.getReceiptName())
                    .date(receipt.getDate())
                    .totalAmount(receipt.getTotalAmount())
                    .isSettled(receipt.isSettled())
                    .build();
            responseList.add(response);
        }

        return ResponseEntity.ok(responseList);
    }
}
