package com.dwin.du.entity.person_product;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person.PersonUpdateService;
import com.dwin.du.entity.person_product.Request.AddPersonProductRequest;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.receipt.ReceiptRepository;
import com.dwin.du.entity.user.User;
import com.dwin.du.service.OperationService;
import com.dwin.du.valid.ValidService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
public class PersonProductService {

    private final PersonProductRepository personProductRepository;
    private final ProductRepository productRepository;
    private final ReceiptRepository receiptRepository;
    private final ValidService valid;
    private final PersonUpdateService updatePerson;
    private final OperationService operation;

    public ResponseEntity<?> addPersonProduct(AddPersonProductRequest request, int productId, String username) {
        User user = valid.validateUser(username);
        Product product = valid.validateProduct(username, productId);
        Person person = valid.validatePerson(username, request.getPersonId());

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        int currentTotalQuantity = personProducts.stream()
                .mapToInt(PersonProduct::getQuantity)
                .sum();

        if (currentTotalQuantity + request.getQuantity() > product.getMaxQuantity()) {
            return ResponseEntity.badRequest().build();
        }

        boolean isCompensation = personProducts.isEmpty();

        PersonProduct personProduct = PersonProduct.builder()
                .user(user)
                .product(product)
                .person(person)
                .settled(product.isSettled())
                .build();

        if (product.isDivisible()) {
            personProduct.setQuantity(request.getQuantity());
            double partPrice = operation.calculatePartPrice(request.getQuantity(), product.getMaxQuantity(), product.getPrice());
            personProduct.setPartOfPrice(partPrice);
        } else {
            personProduct.setQuantity(1);
            personProduct.setPartOfPrice(product.getPrice());
        }

        personProduct.setCompensation(isCompensation);
        personProductRepository.save(personProduct);

        operation.updateCompensationPrice(product);
        updatePerson.updateAllData(username);

        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> removePersonProduct(int personProductId, String username) {
        valid.validateUser(username);
        PersonProduct personProduct = valid.validatePersonProduct(username, personProductId);

        personProductRepository.delete(personProduct);

        Product product = valid.validateProduct(username, personProduct.getProduct().getId());
        operation.updateCompensationPrice(product);
        updatePerson.updateAllData(username);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsSettled(int personProductId, boolean settled, String username) {
        valid.validateUser(username);
        PersonProduct personProduct = valid.validatePersonProduct(username, personProductId);

        personProduct.setSettled(settled);
        personProductRepository.save(personProduct);

        Product product = personProduct.getProduct();
        boolean allPersonProductsSettled = operation.areAllPersonProductsSettled(product);
        product.setSettled(allPersonProductsSettled);
        productRepository.save(product);

        Receipt receipt = product.getReceipt();
        boolean allProductsSettled = operation.areAllProductsSettled(receipt);
        receipt.setSettled(allProductsSettled);
        receiptRepository.save(receipt);

        updatePerson.updateUnpaidAmount(username);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> changePerson(int personProductId, int personId, String username) {
        valid.validateUser(username);
        Person person = valid.validatePerson(username, personId);
        PersonProduct personProduct = valid.validatePersonProduct(username, personProductId);

        personProduct.setPerson(person);
        personProductRepository.save(personProduct);

        updatePerson.updateAllData(username);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsCompensation(int personProductId, String username) {
        valid.validateUser(username);
        PersonProduct personProduct = valid.validatePersonProduct(username, personProductId);

        List<PersonProduct> personProducts = personProductRepository.findByProduct(personProduct.getProduct());

        for (PersonProduct pp : personProducts) {
            if (pp.getId() != personProductId && pp.isCompensation()) {
                pp.setCompensation(false);
                personProductRepository.save(pp);
            }
        }

        updatePerson.updateAllData(username);

        personProduct.setCompensation(true);
        personProductRepository.save(personProduct);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getPersonProductById(int personProductId, String username) {
        valid.validateUser(username);
        PersonProduct personProduct = valid.validatePersonProduct(username, personProductId);

        PersonProductDto response = PersonProductDto.builder()
                .id(personProduct.getId())
                .productId(personProduct.getProduct().getId())
                .personId(personProduct.getPerson().getId())
                .partOfPrice(personProduct.getPartOfPrice())
                .quantity(personProduct.getQuantity())
                .compensation(personProduct.isCompensation())
                .settled(personProduct.isSettled())
                .build();

        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getProductPersonProductsFromProduct(int productId, String username) {
        valid.validateUser(username);
        Product product = valid.validateProduct(username, productId);

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        List<PersonProductDto> responseList = new ArrayList<>();
        for (PersonProduct personProduct : personProducts) {
            PersonProductDto response = PersonProductDto.builder()
                    .id(personProduct.getId())
                    .productId(personProduct.getProduct().getId())
                    .personId(personProduct.getPerson().getId())
                    .partOfPrice(personProduct.getPartOfPrice())
                    .quantity(personProduct.getQuantity())
                    .compensation(personProduct.isCompensation())
                    .settled(personProduct.isSettled())
                    .build();
            responseList.add(response);
        }

        return ResponseEntity.ok(responseList);
    }

    public ResponseEntity<?> getAllProductPersonProductsFromProduct(String username) {
        User user = valid.validateUser(username);

        List<PersonProduct> personProducts = personProductRepository.findByUser(user);
        List<PersonProductDto> responseList = new ArrayList<>();

        for (PersonProduct personProduct : personProducts) {
            PersonProductDto response = PersonProductDto.builder()
                    .id(personProduct.getId())
                    .productId(personProduct.getProduct().getId())
                    .personId(personProduct.getPerson().getId())
                    .partOfPrice(personProduct.getPartOfPrice())
                    .quantity(personProduct.getQuantity())
                    .compensation(personProduct.isCompensation())
                    .settled(personProduct.isSettled())
                    .build();
            responseList.add(response);
        }

        return ResponseEntity.ok(responseList);
    }
}