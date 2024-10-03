package com.dwin.du.entity.person_product;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person.PersonRepository;
import com.dwin.du.entity.person_product.Request.AddPersonProductRequest;
import com.dwin.du.entity.person_product.Response.ShowPersonProductResponse;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.receipt.Request.SetIsSettledRequest;
import com.dwin.du.security.user.User;
import com.dwin.du.security.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class PersonProductService {

    private final UserRepository userRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;
    private final PersonRepository personRepository;

    public ResponseEntity<?> addPersonProduct(AddPersonProductRequest request, int productId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent())
            return ResponseEntity.notFound().build();

        Product product = optionalProduct.get();
        if (!product.getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<Person> optionalPerson = personRepository.findById(request.getPersonId());
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<PersonProduct> existingPersonProduct = personProductRepository.findByProductAndPerson(product, person);
        if (existingPersonProduct.isPresent()) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).build();
        }

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        int currentTotalQuantity = personProducts.stream()
                .mapToInt(PersonProduct::getQuantity)
                .sum();

        if (currentTotalQuantity + request.getQuantity() > product.getMaxQuantity()) {
            return ResponseEntity.badRequest().build();
        }

        boolean isCompensation = personProducts.isEmpty();

        PersonProduct personProduct = PersonProduct.builder()
                .product(product)
                .person(person)
                .maxQuantity(product.getMaxQuantity())
                .isSettled(product.isSettled())
                .build();

        if (product.isDivisible()) {
            personProduct.setQuantity(request.getQuantity());
            double partOfPrice = product.getPrice() * ((double) request.getQuantity() / product.getMaxQuantity());
            personProduct.setPartOfPrice(partOfPrice);
        } else {
            personProduct.setQuantity(1);
            personProduct.setPartOfPrice(product.getPrice());
        }

        personProduct.setCompensation(isCompensation);
        personProductRepository.save(personProduct);

        updateTotalPurchaseAmountForPerson(person);
        updateCompensationAmount(product);
        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> removePersonProduct(int personProductId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<PersonProduct> optionalPersonProduct = personProductRepository.findById(personProductId);
        if (!optionalPersonProduct.isPresent())
            return ResponseEntity.notFound().build();

        PersonProduct personProduct = optionalPersonProduct.get();
        if (!personProduct.getProduct().getReceipt().getUser().getUsername().equals(username)) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        personProductRepository.delete(personProduct);
        updateTotalPurchaseAmountForPerson(personProduct.getPerson());
        updateCompensationAmount(personProduct.getProduct());

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsSettled(int personProductId, SetIsSettledRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<PersonProduct> optionalPersonProduct = personProductRepository.findById(personProductId);
        if (!optionalPersonProduct.isPresent())
            return ResponseEntity.notFound().build();

        PersonProduct personProduct = optionalPersonProduct.get();
        if (!personProduct.getProduct().getReceipt().getUser().getUsername().equals(username)) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        personProduct.setSettled(request.isSettled());
        personProductRepository.save(personProduct);
        updateTotalPurchaseAmountForPerson(personProduct.getPerson());

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsCompensation(int personProductId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<PersonProduct> optionalPersonProduct = personProductRepository.findById(personProductId);
        if (!optionalPersonProduct.isPresent())
            return ResponseEntity.notFound().build();

        PersonProduct personProduct = optionalPersonProduct.get();
        if (!personProduct.getProduct().getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();


        List<PersonProduct> personProducts = personProductRepository.findByProduct(personProduct.getProduct());

        for (PersonProduct pp : personProducts) {
            if (pp.getPersonProductId() != personProductId && pp.isCompensation()) {
                pp.setCompensation(false);
                personProductRepository.save(pp);
            }
        }

        personProduct.setCompensation(true);
        personProductRepository.save(personProduct);
        updateTotalPurchaseAmountForPerson(personProduct.getPerson());
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getPersonProductById(int personProductId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<PersonProduct> optionalPersonProduct = personProductRepository.findById(personProductId);
        if (!optionalPersonProduct.isPresent())
            return ResponseEntity.notFound().build();

        PersonProduct personProduct = optionalPersonProduct.get();
        if (!personProduct.getProduct().getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        ShowPersonProductResponse response = ShowPersonProductResponse.builder()
                .personProductId(personProduct.getPersonProductId())
                .productId(personProduct.getProduct().getProductId())
                .personId(personProduct.getPerson().getPersonId())
                .partOfPrice(personProduct.getPartOfPrice())
                .maxQuantity(personProduct.getMaxQuantity())
                .quantity(personProduct.getQuantity())
                .isCompensation(personProduct.isCompensation())
                .isSettled(personProduct.isSettled())
                .build();
        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getProductPersonProductsFromProduct(int productId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent())
            return ResponseEntity.notFound().build();

        Product product = optionalProduct.get();
        if (!product.getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        List<ShowPersonProductResponse> responseList = new ArrayList<>();
        for (PersonProduct personProduct : personProducts) {
            ShowPersonProductResponse response = ShowPersonProductResponse.builder()
                    .personProductId(personProduct.getPersonProductId())
                    .productId(personProduct.getProduct().getProductId())
                    .personId(personProduct.getPerson().getPersonId())
                    .partOfPrice(personProduct.getPartOfPrice())
                    .quantity(personProduct.getQuantity())
                    .maxQuantity(personProduct.getMaxQuantity())
                    .isCompensation(personProduct.isCompensation())
                    .isSettled(personProduct.isSettled())
                    .build();
            responseList.add(response);
        }
        return ResponseEntity.ok(responseList);
    }

    private void updateTotalPurchaseAmountForPerson(Person person) {
        List<PersonProduct> personProducts = personProductRepository.findByPerson(person);

        double totalPurchaseAmount = 0.0;
        for (PersonProduct personProduct : personProducts) {
            if (!personProduct.isSettled()) {
                totalPurchaseAmount += personProduct.getPartOfPrice();

                if (personProduct.isCompensation()) {
                    totalPurchaseAmount += personProduct.getProduct().getCompensationAmount();
                }
            }
        }

        BigDecimal compensationRounded = new BigDecimal(totalPurchaseAmount).setScale(2, RoundingMode.UP);
        person.setTotalPurchaseAmount(compensationRounded.doubleValue());
        personRepository.save(person);
    }

    public  void updateCompensationAmount(Product product){
        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);

        double totalCompensationAmount = product.getPrice();

        for(PersonProduct personProduct : personProducts){
            totalCompensationAmount -= personProduct.getPartOfPrice();
        }

        BigDecimal compensationRounded = new BigDecimal(totalCompensationAmount).setScale(2, RoundingMode.UP);
        product.setCompensationAmount(compensationRounded.doubleValue());
        productRepository.save(product);
    }
}
