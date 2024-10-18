package com.dwin.du.api.service;
import com.dwin.du.api.dto.PersonDto;
import com.dwin.du.api.entity.Person;
import com.dwin.du.api.repository.PersonRepository;
import com.dwin.du.api.request.AddEditPersonRequest;
import com.dwin.du.api.entity.PersonProduct;
import com.dwin.du.api.repository.PersonProductRepository;
import com.dwin.du.api.entity.Product;
import com.dwin.du.api.repository.ProductRepository;
import com.dwin.du.api.entity.Receipt;
import com.dwin.du.api.entity.User;
import com.dwin.du.validation.ValidationService;
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
    private final ValidationService validator;

    public ResponseEntity<?> addPerson(String username, AddEditPersonRequest request) {
        User user = validator.validateUser(username);
        validator.isNull(request);
        validator.isEmpty(request.getName());

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
        validator.validateUser(username);
        Person person = validator.validatePerson(username, personId);
        validator.isNull(request);
        validator.isEmpty(request.getName());

        person.setName(request.getName());
        person.setSurname(request.getSurname());

        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removePerson(String username, int personId) {
        validator.validateUser(username);
        validator.isNull(personId);
        Person person = validator.validatePerson(username, personId);

        List<PersonProduct> personProducts = personProductRepository.findByPerson(person);
        if (!personProducts.isEmpty())
            return ResponseEntity.status(HttpStatus.CONFLICT).build();

        personRepository.delete(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setReceiptsCounts(String username, int personId, int receiptsCount) {
        validator.validateUser(username);
        validator.isNull(personId);

        Person person = validator.validatePerson(username, personId);
        person.setReceiptsCount(receiptsCount);

        personRepository.save(person);
        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> setTotalAmount(String username, int personId, Double totalAmount) {
        validator.validateUser(username);
        validator.isNull(personId);
        Person person = validator.validatePerson(username, personId);

        person.setTotalAmount(totalAmount);

        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getPerson(String username, int personId) {
        validator.validateUser(username);
        validator.isNull(personId);
        Person person = validator.validatePerson(username, personId);

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
        User user = validator.validateUser(username);

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
        User user = validator.validateUser(username);
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
        validator.validateUser(username);
        Receipt receipt = validator.validateReceipt(username, receiptId);

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
        validator.validateUser(username);
        Product product = validator.validateProduct(username, productId);

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