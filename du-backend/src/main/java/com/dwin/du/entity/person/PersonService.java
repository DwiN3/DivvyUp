package com.dwin.du.entity.person;

import com.dwin.du.entity.person.Request.AddEditPersonRequest;
import com.dwin.du.entity.person.Request.SetReceiptsCountsRequest;
import com.dwin.du.entity.person.Request.SetTotalAmountRequest;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.receipt.ReceiptRepository;
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
public class PersonService {

    private final UserRepository userRepository;
    private final PersonRepository personRepository;
    private final PersonProductRepository personProductRepository;
    private final ProductRepository productRepository;
    private final ReceiptRepository receiptRepository;

    public ResponseEntity<?> addPerson(AddEditPersonRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        User user = userRepository.findByUsername(username).get();
        Person person = Person.builder()
                .user(user)
                .name(request.getName())
                .surname(request.getSurname())
                .receiptsCount(0)
                .totalAmount(0.0)
                .unpaidAmount(0.0)
                .build();
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> editPerson(int personId, AddEditPersonRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        person.setName(request.getName());
        person.setSurname(request.getSurname());
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removePerson(int personId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        List<PersonProduct> personProducts = personProductRepository.findByPerson(person);
        if (!personProducts.isEmpty()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }

        personRepository.delete(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setReceiptsCounts(int personId, SetReceiptsCountsRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        person.setReceiptsCount(request.getReceiptsCount());
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> setTotalAmount(int personId, SetTotalAmountRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        person.setTotalAmount(request.getTotalAmount());
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getPersonById(int personId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        PersonDto response = PersonDto.builder()
                .id(person.getId())
                .name(person.getName())
                .surname(person.getSurname())
                .receiptsCount(person.getReceiptsCount())
                .totalAmount(person.getTotalAmount())
                .unpaidAmount(person.getUnpaidAmount())
                .build();

        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getPersons(String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        User user = optionalUser.get();
        List<Person> persons = personRepository.findByUser(user);
        List<PersonDto> responseList = new ArrayList<>();
        for (Person person : persons) {
            PersonDto response = PersonDto.builder()
                    .id(person.getId())
                    .name(person.getName())
                    .surname(person.getSurname())
                    .receiptsCount(person.getReceiptsCount())
                    .totalAmount(person.getTotalAmount())
                    .unpaidAmount(person.getUnpaidAmount())
                    .build();
            responseList.add(response);

        }
        return ResponseEntity.ok(responseList);
    }

    public ResponseEntity<List<PersonDto>> getPersonsFromReceipt(String username, int receiptId) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent()) {
            return ResponseEntity.notFound().build();
        }

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username)) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

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
                    .totalAmount(person.getTotalAmount())
                    .unpaidAmount(person.getUnpaidAmount())
                    .build();
            personsSet.add(personDto);
        }

        List<PersonDto> persons = new ArrayList<>(personsSet);

        return ResponseEntity.ok(persons);
    }

    public ResponseEntity<?> getPersonsFromProduct(String username, int productId) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent()) {
            return ResponseEntity.notFound().build();
        }

        Product product = optionalProduct.get();

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        List<PersonDto> persons = personProducts.stream()
                .map(pp -> {
                    Person person = pp.getPerson();
                    return PersonDto.builder()
                            .id(person.getId())
                            .name(person.getName())
                            .surname(person.getSurname())
                            .receiptsCount(person.getReceiptsCount())
                            .totalAmount(person.getTotalAmount())
                            .unpaidAmount(person.getUnpaidAmount())
                            .build();
                })
                .collect(Collectors.toList());

        return ResponseEntity.ok(persons);
    }
}