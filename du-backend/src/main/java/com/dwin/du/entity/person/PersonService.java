package com.dwin.du.entity.person;

import com.dwin.du.entity.person.Request.AddEditPersonRequest;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.user.User;
import com.dwin.du.valid.ValidService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.util.*;
import java.util.stream.Collectors;


@Service
@RequiredArgsConstructor
public class PersonService {

    private final PersonRepository personRepository;
    private final PersonProductRepository personProductRepository;
    private final ProductRepository productRepository;
    private final ValidService valid;

    public ResponseEntity<?> addPerson(String username, AddEditPersonRequest request) {
        User user = valid.validateUser(username);
        valid.isNull(request);
        valid.isEmpty(request.getName());

        Person person = Person.builder()
                .user(user)
                .name(request.getName())
                .surname(request.getSurname())
                .receiptsCount(0)
                .productsCount(0)
                .totalAmount(0.0)
                .unpaidAmount(0.0)
                .userAccount(false)
                .build();
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> editPerson(String username, int personId, AddEditPersonRequest request) {
        valid.validateUser(username);
        Person person = valid.validatePerson(username, personId);
        valid.isNull(request);
        valid.isEmpty(request.getName());

        person.setName(request.getName());
        person.setSurname(request.getSurname());
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removePerson(String username, int personId) {
        valid.validateUser(username);
        valid.isNull(personId);
        Person person = valid.validatePerson(username, personId);

        List<PersonProduct> personProducts = personProductRepository.findByPerson(person);
        if (!personProducts.isEmpty()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }

        personRepository.delete(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setReceiptsCounts(String username, int personId, int receiptsCount) {
        valid.validateUser(username);
        valid.isNull(personId);
        Person person = valid.validatePerson(username, personId);

        person.setReceiptsCount(receiptsCount);
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> setTotalAmount(String username, int personId, Double totalAmount) {
        valid.validateUser(username);
        valid.isNull(personId);
        Person person = valid.validatePerson(username, personId);

        person.setTotalAmount(totalAmount);
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getPerson(String username, int personId) {
        valid.validateUser(username);
        valid.isNull(personId);
        Person person = valid.validatePerson(username, personId);

        PersonDto response = PersonDto.builder()
                .id(person.getId())
                .name(person.getName())
                .surname(person.getSurname())
                .receiptsCount(person.getReceiptsCount())
                .productsCount(person.getProductsCount())
                .totalAmount(person.getTotalAmount())
                .unpaidAmount(person.getUnpaidAmount())
                .userAccount(person.isUserAccount())
                .build();

        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getPersons(String username) {
        User user = valid.validateUser(username);

        List<Person> persons = personRepository.findByUser(user);
        List<PersonDto> responseList = new ArrayList<>();
        for (Person person : persons) {
            PersonDto response = PersonDto.builder()
                    .id(person.getId())
                    .name(person.getName())
                    .surname(person.getSurname())
                    .receiptsCount(person.getReceiptsCount())
                    .productsCount(person.getProductsCount())
                    .totalAmount(person.getTotalAmount())
                    .unpaidAmount(person.getUnpaidAmount())
                    .userAccount(person.isUserAccount())
                    .build();
            responseList.add(response);

        }
        return ResponseEntity.ok(responseList);
    }

    public ResponseEntity<?> getUserPerson(String username) {
        User user = valid.validateUser(username);
        List<Person> persons = personRepository.findByUser(user);

        for (Person person : persons) {
            if (person.isUserAccount()) {
                PersonDto response = PersonDto.builder()
                        .id(person.getId())
                        .name(person.getName())
                        .surname(person.getSurname())
                        .receiptsCount(person.getReceiptsCount())
                        .productsCount(person.getProductsCount())
                        .totalAmount(person.getTotalAmount())
                        .unpaidAmount(person.getUnpaidAmount())
                        .userAccount(person.isUserAccount())
                        .build();

                return ResponseEntity.ok(response);
            }
        }

        return ResponseEntity.notFound().build();
    }


    public ResponseEntity<List<PersonDto>> getPersonsFromReceipt(String username, int receiptId) {
        valid.validateUser(username);
        Receipt receipt = valid.validateReceipt(username, receiptId);

        List<Product> products = productRepository.findByReceipt(receipt);

        List<PersonProduct> personProducts = new ArrayList<>();
        for (Product product : products) {
            List<PersonProduct> ppList = personProductRepository.findByProduct(product);
            personProducts.addAll(ppList);
        }

        Set<PersonDto> personsSet = new HashSet<>();
        for (PersonProduct pp : personProducts) {
            Person person = pp.getPerson();
            PersonDto personDto = PersonDto.builder()
                    .id(person.getId())
                    .name(person.getName())
                    .surname(person.getSurname())
                    .receiptsCount(person.getReceiptsCount())
                    .productsCount(person.getProductsCount())
                    .totalAmount(person.getTotalAmount())
                    .unpaidAmount(person.getUnpaidAmount())
                    .userAccount(person.isUserAccount())
                    .build();
            personsSet.add(personDto);
        }

        List<PersonDto> persons = new ArrayList<>(personsSet);

        return ResponseEntity.ok(persons);
    }

    public ResponseEntity<?> getPersonsFromProduct(String username, int productId) {
        valid.validateUser(username);
        Product product = valid.validateProduct(username, productId);

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        List<PersonDto> persons = personProducts.stream()
                .map(pp -> {
                    Person person = pp.getPerson();
                    return PersonDto.builder()
                            .id(person.getId())
                            .name(person.getName())
                            .surname(person.getSurname())
                            .receiptsCount(person.getReceiptsCount())
                            .productsCount(person.getProductsCount())
                            .totalAmount(person.getTotalAmount())
                            .unpaidAmount(person.getUnpaidAmount())
                            .userAccount(person.isUserAccount())
                            .build();
                })
                .collect(Collectors.toList());

        return ResponseEntity.ok(persons);
    }
}