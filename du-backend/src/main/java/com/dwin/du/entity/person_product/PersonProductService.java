package com.dwin.du.entity.person_product;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person.PersonRepository;
import com.dwin.du.entity.person_product.Request.AddPersonProductRequest;
import com.dwin.du.entity.person_product.Request.ChangePersonRequest;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.receipt.Request.SetSettledRequest;
import com.dwin.du.entity.user.User;
import com.dwin.du.entity.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.parameters.P;
import org.springframework.stereotype.Service;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class PersonProductService {

    private final UserRepository userRepository;
    private final PersonRepository personRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;

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
            double partPrice = calculatePartPrice(request.getQuantity(), product.getMaxQuantity(), product.getPrice());
            personProduct.setPartOfPrice(partPrice);
        } else {
            personProduct.setQuantity(1);
            personProduct.setPartOfPrice(product.getPrice());
        }

        personProduct.setCompensation(isCompensation);
        personProductRepository.save(personProduct);

        updateCompensationPrice(product);

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

        Optional<Product> optionalProduct = productRepository.findById(personProduct.getProduct().getId());
        if (!optionalProduct.isPresent())
            return ResponseEntity.notFound().build();
        Product product = optionalProduct.get();
        updateCompensationPrice(product);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsSettled(int personProductId, SetSettledRequest request, String username) {
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

        personProduct.setSettled(request.isSettled);
        personProductRepository.save(personProduct);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> changePerson(int personProductId, ChangePersonRequest request, String username) {
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

        Optional<Person> optionalPerson = personRepository.findById(request.getPersonId());
        if (!optionalPerson.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Person person = optionalPerson.get();
        personProduct.setPerson(person);
        personProductRepository.save(personProduct);

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
            if (pp.getId() != personProductId && pp.isCompensation()) {
                pp.setCompensation(false);
                personProductRepository.save(pp);
            }
        }

        personProduct.setCompensation(true);
        personProductRepository.save(personProduct);

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

        PersonProductDto response = PersonProductDto.builder()
                .id(personProduct.getId())
                .productId(personProduct.getProduct().getId())
                .personId(personProduct.getPerson().getId())
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
        List<PersonProductDto> responseList = new ArrayList<>();
        for (PersonProduct personProduct : personProducts) {
            PersonProductDto response = PersonProductDto.builder()
                    .id(personProduct.getId())
                    .productId(personProduct.getProduct().getId())
                    .personId(personProduct.getPerson().getId())
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

    public ResponseEntity<?> getAllProductPersonProductsFromProduct(String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        User user = optionalUser.get();
        List<PersonProduct> personProducts = personProductRepository.findByUser(user);
        List<PersonProductDto> responseList = new ArrayList<>();

        for (PersonProduct personProduct : personProducts) {
            PersonProductDto response = PersonProductDto.builder()
                    .id(personProduct.getId())
                    .productId(personProduct.getProduct().getId())
                    .personId(personProduct.getPerson().getId())
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

    private double calculatePartPrice(int quantity, int maxQuantity, double price) {
        double partPrice = ((double) quantity / maxQuantity) * price;
        return partPrice;
    }

    private double calculateCompensationPrice(List<PersonProduct> personProductDtos, double price){
        double compensationPrice = 0;
        for(var item : personProductDtos){
            compensationPrice += item.getPartOfPrice();
        }
        compensationPrice = price - compensationPrice;
        return compensationPrice;
    }

    private void updateCompensationPrice(Product product) {
        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        double compensationPrice = calculateCompensationPrice(personProducts, product.getPrice());
        product.setCompensationPrice(compensationPrice);
        productRepository.save(product);
    }
}