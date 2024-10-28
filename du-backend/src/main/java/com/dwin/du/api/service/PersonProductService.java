package com.dwin.du.api.service;
import com.dwin.du.api.dto.PersonProductDto;
import com.dwin.du.api.entity.*;
import com.dwin.du.api.repository.PersonProductRepository;
import com.dwin.du.api.repository.PersonRepository;
import com.dwin.du.api.request.AddEditPersonProductRequest;
import com.dwin.du.api.repository.ProductRepository;
import com.dwin.du.api.repository.ReceiptRepository;
import com.dwin.du.update.EntityUpdateService;
import com.dwin.du.validation.ValidationException;
import com.dwin.du.validation.ValidationService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
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
    private final PersonRepository personRepository;
    private final ValidationService validator;
    private final EntityUpdateService updater;

    public ResponseEntity<?> addPersonProduct(String username, int productId, AddEditPersonProductRequest request) {
        try {
            User user = validator.validateUser(username);
            validator.isNull(request, "Nie przekazano danych");
            validator.isNull(request.getPersonId(), "Brak identyfikatora osoby");
            validator.isNull(request.getQuantity(), "Ilość jest wymagana");
            validator.isNull(productId, "Brak identyfikatora produktu");
            Product product = validator.validateProduct(username, productId);
            Person person = validator.validatePerson(username, request.getPersonId());

            List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
            int currentTotalQuantity = personProducts.stream()
                    .mapToInt(PersonProduct::getQuantity)
                    .sum();

            var personProductExists = personProductRepository.findByProductAndPerson(product, person);
            if (!personProductExists.isEmpty())
                return ResponseEntity.status(HttpStatus.CONFLICT).build();

            if (currentTotalQuantity + request.getQuantity() > product.getMaxQuantity()) {
                return ResponseEntity.badRequest().build();
            }

            boolean isCompensation = personProducts.isEmpty();

            PersonProduct personProduct = PersonProduct.builder()
                    .product(product)
                    .person(person)
                    .settled(product.isSettled())
                    .build();

            if (product.isDivisible()) {
                personProduct.setQuantity(request.getQuantity());
                double partPrice = updater.calculatePartPrice(request.getQuantity(), product.getMaxQuantity(), product.getPrice());
                personProduct.setPartOfPrice(partPrice);
            } else {
                personProduct.setQuantity(1);
                personProduct.setPartOfPrice(product.getPrice());
            }

            personProduct.setCompensation(isCompensation);

            personProductRepository.save(personProduct);
            updater.updateCompensationPrice(product);
            updater.updatePerson(username, false);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> editPersonProduct(String username, int personProductId, AddEditPersonProductRequest request) {
        try {
            validator.validateUser(username);
            validator.isNull(request, "Nie przekazano danych");
            validator.isNull(request.getPersonId(),"Brak identyfikatora osoby");
            validator.isNull(request.getQuantity(), "Ilość jest wymagana");
            validator.isNull(personProductId, "Brak identyfikatora powiązania osoby z produktem");
            PersonProduct personProduct = validator.validatePersonProduct(username, personProductId);
            Product product = validator.validateProduct(username, personProduct.getProduct().getId());
            Person person = validator.validatePerson(username, request.getPersonId());

            personProduct.setPerson(person);
            if (product.isDivisible()) {
                personProduct.setQuantity(request.getQuantity());
                double partPrice = updater.calculatePartPrice(request.getQuantity(), product.getMaxQuantity(), product.getPrice());
                personProduct.setPartOfPrice(partPrice);
            } else {
                personProduct.setQuantity(1);
                personProduct.setPartOfPrice(product.getPrice());
            }

            personProductRepository.save(personProduct);
            updater.updateCompensationPrice(product);
            updater.updatePerson(username, false);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }


    public ResponseEntity<?> removePersonProduct(String username, int personProductId) {
        try {
            validator.validateUser(username);
            validator.isNull(personProductId, "Brak identyfikatora powiązania osoby z produktem");
            PersonProduct personProduct = validator.validatePersonProduct(username, personProductId);

            personProductRepository.delete(personProduct);

            Product product = validator.validateProduct(username, personProduct.getProduct().getId());
            updater.updateCompensationPrice(product);
            updater.updatePerson(username, false);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> setIsSettled(String username, int personProductId, boolean settled) {
        try {
            validator.validateUser(username);
            validator.isNull(personProductId, "Brak identyfikatora powiązania osoby z produktem");
            PersonProduct personProduct = validator.validatePersonProduct(username, personProductId);

            personProduct.setSettled(settled);
            personProductRepository.save(personProduct);

            Product product = personProduct.getProduct();
            boolean allPersonProductsSettled = updater.areAllPersonProductsSettled(product);
            product.setSettled(allPersonProductsSettled);
            productRepository.save(product);

            Receipt receipt = product.getReceipt();
            boolean allProductsSettled = updater.areAllProductsSettled(receipt);
            receipt.setSettled(allProductsSettled);

            receiptRepository.save(receipt);
            updater.updatePerson(username, false);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> setPerson(String username, int personProductId, int personId) {
        try {
            validator.validateUser(username);
            validator.isNull(personProductId, "Brak identyfikatora powiązania osoby z produktem");
            validator.isNull(personId, "Brak identyfikatora osoby");
            Person person = validator.validatePerson(username, personId);
            PersonProduct personProduct = validator.validatePersonProduct(username, personProductId);

            var personProductExists = personProductRepository.findByProductAndPerson(personProduct.getProduct(), person);
            if (!personProductExists.isEmpty())
                return ResponseEntity.status(HttpStatus.CONFLICT).build();

            personProduct.setPerson(person);

            personProductRepository.save(personProduct);
            updater.updatePerson(username, false);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> setIsCompensation(String username, int personProductId) {
        try {
            validator.validateUser(username);
            validator.isNull(personProductId, "Brak identyfikatora powiązania osoby z produktem");
            PersonProduct personProduct = validator.validatePersonProduct(username, personProductId);

            List<PersonProduct> personProducts = personProductRepository.findByProduct(personProduct.getProduct());
            for (PersonProduct pp : personProducts) {
                if (pp.getId() != personProductId && pp.isCompensation()) {
                    pp.setCompensation(false);
                    personProductRepository.save(pp);
                }
            }
            personProduct.setCompensation(true);

            personProductRepository.save(personProduct);
            updater.updatePerson(username, false);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getPersonProduct(String username, int personProductId) {
        try {
            validator.validateUser(username);
            validator.isNull(personProductId, "Brak identyfikatora powiązania osoby z produktem");
            PersonProduct personProduct = validator.validatePersonProduct(username, personProductId);

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

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getPersonProductsFromProduct(String username, int productId) {
        try {
            validator.validateUser(username);
            validator.isNull(productId, "Brak identyfikatora produktu");
            Product product = validator.validateProduct(username, productId);

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

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getAllPersonProducts(String username) {
        try {
            User user = validator.validateUser(username);

            List<Person> persons = personRepository.findByUser(user);
            List<PersonProduct> personProducts = personProductRepository.findByPersonIn(persons);
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