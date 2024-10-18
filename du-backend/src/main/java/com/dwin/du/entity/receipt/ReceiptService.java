package com.dwin.du.entity.receipt;
import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.receipt.Request.AddEditReceiptRequest;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.user.User;
import com.dwin.du.service.EntityUpdateService;
import com.dwin.du.validation.ValidationService;
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
    private final EntityUpdateService updater;
    private final ValidationService validator;

    public ResponseEntity<?> addReceipt(String username, AddEditReceiptRequest request) {
        User user = validator.validateUser(username);
        validator.isNull(request);
        validator.isEmpty(request.getName());

        Receipt receipt = Receipt.builder()
                .user(user)
                .name(request.getName())
                .date(request.getDate())
                .totalPrice(0.0)
                .settled(false)
                .build();

        receiptRepository.save(receipt);
        updater.updatePerson(username);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> editReceipt(String username, int receiptId, AddEditReceiptRequest request) {
        validator.validateUser(username);
        validator.isNull(request);
        validator.isEmpty(request.getName());
        Receipt receipt = validator.validateReceipt(username, receiptId);

        receipt.setName(request.getName());
        receipt.setDate(request.getDate());

        receiptRepository.save(receipt);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removeReceipt(String username, int receiptId) {
        validator.validateUser(username);
        validator.isNull(receiptId);
        Receipt receipt = validator.validateReceipt(username, receiptId);

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
        updater.updatePerson(username);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setTotalPrice(String username, int receiptId, Double totalPrice) {
        validator.validateUser(username);
        validator.isNull(receiptId);
        validator.isNull(totalPrice);
        Receipt receipt = validator.validateReceipt(username, receiptId);

        receipt.setTotalPrice(totalPrice);

        receiptRepository.save(receipt);
        updater.updatePerson(username);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsSettled(String username, int receiptId, boolean settled) {
        validator.validateUser(username);
        validator.isNull(receiptId);
        Receipt receipt = validator.validateReceipt(username, receiptId);

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

        updater.updatePerson(username);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getReceipt(String username, int receiptId) {
        validator.validateUser(username);
        validator.isNull(receiptId);
        Receipt receipt = validator.validateReceipt(username, receiptId);

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
        User user = validator.validateUser(username);

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
