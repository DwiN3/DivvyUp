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
import com.dwin.du.validation.ValidationException;
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
        try {
            User user = validator.validateUser(username);
            validator.isNull(request, "Nie przekazano danych");
            validator.isEmpty(request.getName(), "Nazwa osoby jest wymagana");

            Person person = Person.builder()
                    .user(user)
                    .name(request.getName())
                    .surname(request.getSurname())
                    .receiptsCount(0)
                    .productsCount(0)
                    .totalAmount(0.0)
                    .unpaidAmount(0.0)
                    .loanBalance(0.0)
                    .userAccount(false)
                    .build();

            personRepository.save(person);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> editPerson(String username, int personId, AddEditPersonRequest request) {
        try {
            validator.validateUser(username);
            Person person = validator.validatePerson(username, personId);
            validator.isNull(request, "Nie przekazano danych");
            validator.isEmpty(request.getName(), "Nazwa osoby jest wymagana");

            person.setName(request.getName());
            person.setSurname(request.getSurname());

            personRepository.save(person);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> removePerson(String username, int personId) throws ValidationException {
        try {
            validator.validateUser(username);
            validator.isNull(personId, "Brak identyfikatora osoby");
            Person person = validator.validatePerson(username, personId);

            List<PersonProduct> personProducts = personProductRepository.findByPerson(person);
            if (!personProducts.isEmpty())
                throw new ValidationException(HttpStatus.CONFLICT, "Nie można usunąć osoby która posiada przypisane produkty");

            personRepository.delete(person);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> setReceiptsCounts(String username, int personId, int receiptsCount) {
        try {
            validator.validateUser(username);
            validator.isNull(personId, "Brak identyfikatora osoby");

            Person person = validator.validatePerson(username, personId);
            person.setReceiptsCount(receiptsCount);

            personRepository.save(person);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }


    public ResponseEntity<?> setTotalAmount(String username, int personId, Double totalAmount) {
        try {
            validator.validateUser(username);
            validator.isNull(personId, "Brak identyfikatora osoby");
            Person person = validator.validatePerson(username, personId);

            person.setTotalAmount(totalAmount);

            personRepository.save(person);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getPerson(String username, int personId) {
        try {
            validator.validateUser(username);
            Person person = validator.validatePerson(username, personId);

            PersonDto response = PersonDto.builder()
                    .id(person.getId())
                    .name(person.getName())
                    .surname(person.getSurname())
                    .receiptsCount(person.getReceiptsCount())
                    .productsCount(person.getProductsCount())
                    .totalAmount(person.getTotalAmount())
                    .unpaidAmount(person.getUnpaidAmount())
                    .loanBalance(person.getLoanBalance())
                    .userAccount(person.isUserAccount())
                    .build();

            return ResponseEntity.ok(response);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getPersons(String username) {
        try {
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
                        .loanBalance(person.getLoanBalance())
                        .userAccount(person.isUserAccount())
                        .build();
                responseList.add(response);
            }

            return ResponseEntity.ok(responseList);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getUserPerson(String username) {
        try {
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
                            .loanBalance(person.getLoanBalance())
                            .userAccount(person.isUserAccount())
                            .build();

                    return ResponseEntity.ok(response);
                }
            }

            return ResponseEntity.notFound().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }


    public ResponseEntity<?> getPersonsFromReceipt(String username, int receiptId) {
        try {
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
                        .loanBalance(person.getLoanBalance())
                        .userAccount(person.isUserAccount())
                        .build();
                personsSet.add(personDto);
            }
            List<PersonDto> persons = new ArrayList<>(personsSet);

            return ResponseEntity.ok(persons);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getPersonsFromProduct(String username, int productId) {
        try {
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
                                .loanBalance(person.getLoanBalance())
                                .userAccount(person.isUserAccount())
                                .build();
                    })
                    .collect(Collectors.toList());

            return ResponseEntity.ok(persons);

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