package com.dwin.du.entity.receipt;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person.PersonRepository;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.receipt.Request.AddReceiptRequest;
import com.dwin.du.entity.receipt.Request.SetIsSettledRequest;
import com.dwin.du.entity.receipt.Request.SetTotalAmountReceiptRequest;
import com.dwin.du.entity.receipt.Response.ReceiptDto;
import com.dwin.du.entity.user.User;
import com.dwin.du.entity.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.*;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class ReceiptService {

    private final ReceiptRepository receiptRepository;
    private final UserRepository userRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;
    private final PersonRepository personRepository;

    public ResponseEntity<?> addReceipt(AddReceiptRequest request, String username) {
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

    public ResponseEntity<?> editReceipt(int receiptId, AddReceiptRequest request, String username) {
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

        List<Product> products = productRepository.findByReceipt(receipt);

        Set<Person> involvedPersons = new HashSet<>();
        for (Product product : products) {
            List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
            involvedPersons.addAll(
                    personProducts.stream().map(PersonProduct::getPerson).collect(Collectors.toSet())
            );

            personProductRepository.deleteAll(personProducts);
        }

        productRepository.deleteAll(products);
        receiptRepository.delete(receipt);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setTotalAmount(int receiptId, SetTotalAmountReceiptRequest request, String username) {
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

    public ResponseEntity<?> setIsSettled(int receiptId, SetIsSettledRequest request, String username) {
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

        List<Product> products = productRepository.findByReceipt(receipt);
        for (Product product : products) {
            product.setSettled(request.isSettled());
            productRepository.save(product);

            List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
            for (PersonProduct personProduct : personProducts) {
                personProduct.setSettled(request.isSettled());
                personProductRepository.save(personProduct);
            }
        }

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
