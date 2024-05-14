package com.dwin.rm.entity.receipt;

import com.dwin.rm.entity.receipt.Request.AddReceiptRequest;
import com.dwin.rm.entity.receipt.Request.SetTotalAmountReceiptRequest;
import com.dwin.rm.entity.receipt.Request.ShowReceiptRequest;
import com.dwin.rm.security.user.User;
import com.dwin.rm.security.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class ReceiptService {

    private final ReceiptRepository receiptRepository;
    private final UserRepository userRepository;

    public ResponseEntity<?> addReceipt(AddReceiptRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Receipt receipt = Receipt.builder()
                    .user(user)
                    .receiptName(request.getReceiptName())
                    .date(request.getDate())
                    .totalAmount(0.0)
                    .isSettled(false)
                    .build();
            receiptRepository.save(receipt);
            return ResponseEntity.ok().build();
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> editReceipt(int receiptId, AddReceiptRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
            if (optionalReceipt.isPresent()) {
                Receipt receipt = optionalReceipt.get();
                if (receipt.getUser().getUsername().equals(username)) {
                    receipt.setReceiptName(request.getReceiptName());
                    receipt.setDate(request.getDate());
                    receiptRepository.save(receipt);
                    return ResponseEntity.ok().build();
                } else {
                    return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
                }
            } else {
                return ResponseEntity.notFound().build();
            }
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> removeReceipt(int receiptId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
            if (optionalReceipt.isPresent()) {
                Receipt receipt = optionalReceipt.get();
                if (receipt.getUser().getUsername().equals(username)) {
                    receiptRepository.delete(receipt);
                    return ResponseEntity.ok().build();
                } else {
                    return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
                }
            } else {
                return ResponseEntity.notFound().build();
            }
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> setTotalAmount(int receiptId, SetTotalAmountReceiptRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
            if (optionalReceipt.isPresent()) {
                Receipt receipt = optionalReceipt.get();
                if (receipt.getUser().getUsername().equals(username)) {
                    receipt.setTotalAmount(request.getTotalAmount());
                    receiptRepository.save(receipt);
                    return ResponseEntity.ok().build();
                } else {
                    return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
                }
            } else {
                return ResponseEntity.notFound().build();
            }
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> showReceiptById(int receiptId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
            if (optionalReceipt.isPresent()) {
                Receipt receipt = optionalReceipt.get();
                    if (receipt.getUser().getUsername().equals(username)) {
                        ShowReceiptRequest response = ShowReceiptRequest.builder()
                                .receiptId(receipt.getReceiptId())
                                .receiptName(receipt.getReceiptName())
                                .date(receipt.getDate())
                                .totalAmount(receipt.getTotalAmount())
                                .isSettled(receipt.isSettled())
                                .build();
                    return ResponseEntity.ok(response);
                } else {
                    return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
                }
            } else {
                return ResponseEntity.notFound().build();
            }
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> showReceipts(String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            List<Receipt> receipts = receiptRepository.findByUser(user);
            List<ShowReceiptRequest> responseList = new ArrayList<>();
            for (Receipt receipt : receipts) {
                ShowReceiptRequest response = ShowReceiptRequest.builder()
                        .receiptId(receipt.getReceiptId())
                        .receiptName(receipt.getReceiptName())
                        .date(receipt.getDate())
                        .totalAmount(receipt.getTotalAmount())
                        .isSettled(receipt.isSettled())
                        .build();
                responseList.add(response);
            }
            return ResponseEntity.ok(responseList);
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }
}
