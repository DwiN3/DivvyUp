package com.dwin.du.api.service;
import com.dwin.du.api.dto.ReceiptDto;
import com.dwin.du.api.entity.*;
import com.dwin.du.api.repository.ReceiptRepository;
import com.dwin.du.api.request.AddEditReceiptRequest;
import com.dwin.du.api.repository.PersonProductRepository;
import com.dwin.du.api.repository.ProductRepository;
import com.dwin.du.update.EntityUpdateService;
import com.dwin.du.validation.ValidationException;
import com.dwin.du.validation.ValidationService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.text.ParseException;
import java.text.SimpleDateFormat;
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
        try {
            User user = validator.validateUser(username);
            validator.isNull(request, "Nie przekazano danych");
            validator.isEmpty(request.getName(), "Nazwa rachunku jest wymagana");

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

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> editReceipt(String username, int receiptId, AddEditReceiptRequest request) {
        try {
            validator.validateUser(username);
            validator.isNull(request, "Nie przekazano danych");
            validator.isEmpty(request.getName(), "Nazwa rachunku jest wymagana");
            Receipt receipt = validator.validateReceipt(username, receiptId);

            receipt.setName(request.getName());
            receipt.setDate(request.getDate());

            receiptRepository.save(receipt);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> removeReceipt(String username, int receiptId) {
        try {
            validator.validateUser(username);
            validator.isNull(receiptId, "Brak identyfikatora rachunku");
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

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> setTotalPrice(String username, int receiptId, Double totalPrice) {
        try {
            validator.validateUser(username);
            validator.isNull(receiptId, "Brak identyfikatora rachunku");
            validator.isNull(totalPrice, "Kwota łączna jest wymagana");
            Receipt receipt = validator.validateReceipt(username, receiptId);

            receipt.setTotalPrice(totalPrice);

            receiptRepository.save(receipt);
            updater.updatePerson(username);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> setIsSettled(String username, int receiptId, boolean settled) {
        try {
            validator.validateUser(username);
            validator.isNull(receiptId, "Brak identyfikatora rachunku");
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

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getReceipt(String username, int receiptId) {
        try {
            validator.validateUser(username);
            validator.isNull(receiptId, "Brak identyfikatora rachunku");
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

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getReceipts(String username) {
        try {
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

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getReceiptsByDataRange(String username, String fromDate, String toDate) throws ValidationException {
        try {
            User user = validator.validateUser(username);
            validator.isEmpty(fromDate, "Data początkowa jest wymagana");
            validator.isEmpty(toDate, "Data końcowa jest wymagana");

            SimpleDateFormat dateFormat = new SimpleDateFormat("dd-MM-yyyy");
            Date dateFrom = dateFormat.parse(fromDate);
            Date dateTo = dateFormat.parse(toDate);

            dateFrom = new Date(dateFrom.getTime());
            dateFrom.setHours(0);
            dateFrom.setMinutes(0);
            dateFrom.setSeconds(0);

            dateTo = new Date(dateTo.getTime());
            dateTo.setHours(23);
            dateTo.setMinutes(59);
            dateTo.setSeconds(59);

            List<Receipt> receipts = receiptRepository.findByUserAndDateBetween(user, dateFrom, dateTo);
            List<ReceiptDto> responseList = receipts.stream()
                    .map(receipt -> ReceiptDto.builder()
                            .id(receipt.getId())
                            .name(receipt.getName())
                            .date(receipt.getDate())
                            .totalPrice(receipt.getTotalPrice())
                            .isSettled(receipt.isSettled())
                            .build())
                    .collect(Collectors.toList());

            return ResponseEntity.ok(responseList);

        } catch (Exception e) {
            return handleException(e);
        }
    }


    private ResponseEntity<?> handleException(Exception e) {
        if (e instanceof ValidationException) {
            HttpStatus status = ((ValidationException) e).getStatus();
            return ResponseEntity.status(status).body(e.getMessage());
        }
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Wystąpił nieoczekiwany błąd.");
    }
}
