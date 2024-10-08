package com.dwin.du.entity.receipt;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.receipt.Request.AddEditReceiptRequest;
import com.dwin.du.entity.user.User;
import com.dwin.du.valid.ValidService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.util.*;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class ReceiptService {

    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;
    private final ValidService valid;

    public ResponseEntity<?> addReceipt(AddEditReceiptRequest request, String username) {
        User user = valid.validateUser(username);

        Receipt receipt = Receipt.builder()
                .user(user)
                .name(request.getName())
                .date(request.getDate())
                .totalPrice(0.0)
                .settled(false)
                .build();

        receiptRepository.save(receipt);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> editReceipt(int receiptId, AddEditReceiptRequest request, String username) {
        valid.validateUser(username);
        Receipt receipt = valid.validateReceipt(username, receiptId);

        receipt.setName(request.getName());
        receipt.setDate(request.getDate());
        receiptRepository.save(receipt);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removeReceipt(int receiptId, String username) {
        valid.validateUser(username);
        Receipt receipt = valid.validateReceipt(username, receiptId);

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

    public ResponseEntity<?> setTotalPrice(int receiptId, Double totalPrice, String username) {
        valid.validateUser(username);
        Receipt receipt = valid.validateReceipt(username, receiptId);

        receipt.setTotalPrice(totalPrice);
        receiptRepository.save(receipt);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsSettled(int receiptId, boolean settled, String username) {
        valid.validateUser(username);
        Receipt receipt = valid.validateReceipt(username, receiptId);

        receipt.setSettled(settled);
        receiptRepository.save(receipt);

        List<Product> products = productRepository.findByReceipt(receipt);
        for (Product product : products) {
            product.setSettled(settled);
            productRepository.save(product);

            List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
            for (PersonProduct personProduct : personProducts) {
                personProduct.setSettled(settled);
                personProductRepository.save(personProduct);
            }
        }

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getReceiptById(int receiptId, String username) {
        valid.validateUser(username);
        Receipt receipt = valid.validateReceipt(username, receiptId);

        Receipt response = Receipt.builder()
                .id(receipt.getId())
                .name(receipt.getName())
                .date(receipt.getDate())
                .totalPrice(receipt.getTotalPrice())
                .settled(receipt.isSettled())
                .user(receipt.getUser())
                .build();
        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getReceipts(String username) {
        User user = valid.validateUser(username);

        List<Receipt> receipts = receiptRepository.findByUser(user);
        List<ReceiptDto> responseList = new ArrayList<>();
        for (Receipt receipt : receipts) {
            ReceiptDto response = ReceiptDto.builder()
                    .id(receipt.getId())
                    .name(receipt.getName())
                    .date(receipt.getDate())
                    .totalPrice(receipt.getTotalPrice())
                    .isSettled(receipt.isSettled())
                    .build();
            responseList.add(response);
        }

        return ResponseEntity.ok(responseList);
    }
}
