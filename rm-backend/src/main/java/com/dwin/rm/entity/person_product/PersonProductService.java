package com.dwin.rm.entity.person_product;

import com.dwin.rm.entity.person.Person;
import com.dwin.rm.entity.person.PersonRepository;
import com.dwin.rm.entity.person_product.Request.AddPersonProductRequest;
import com.dwin.rm.entity.person_product.Response.ShowPersonProductResponse;
import com.dwin.rm.entity.product.Product;
import com.dwin.rm.entity.product.ProductRepository;
import com.dwin.rm.security.user.User;
import com.dwin.rm.security.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class PersonProductService {

    private final PersonProductRepository personProductRepository;
    private final ProductRepository productRepository;
    private final UserRepository userRepository;
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

        PersonProduct personProduct;
        if(product.isDivisible()){
            personProduct = PersonProduct.builder()
                    .product(product)
                    .person(person)
                    .partOfPrice(request.getPartOfPrice())
                    .quantity(request.getQuantity())
                    .maxQuantity(product.getMaxQuantity())
                    .isCompensation(request.isCompensation())
                    .compensationAmount(request.getCompensationAmount())
                    .isSettled(false)
                    .build();
        }
        else{
            personProduct = PersonProduct.builder()
                    .product(product)
                    .person(person)
                    .partOfPrice(1)
                    .quantity(1)
                    .maxQuantity(1)
                    .isCompensation(true)
                    .compensationAmount(request.getCompensationAmount())
                    .isSettled(false)
                    .build();
        }

        personProductRepository.save(personProduct);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removePersonProduct(int personProductId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<PersonProduct> optionalPersonProduct = personProductRepository.findById(personProductId);
        if (!optionalPersonProduct.isPresent())
            return ResponseEntity.notFound().build();

        PersonProduct personProduct = optionalPersonProduct.get();
        if (!personProduct.getProduct().getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        personProductRepository.delete(personProduct);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> showPersonProduct(int personProductId, String username) {
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
                .quantity(personProduct.getQuantity())
                .isCompensation(personProduct.isCompensation())
                .compensationAmount(personProduct.getCompensationAmount())
                .isSettled(personProduct.isSettled())
                .build();
        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> showPersonProducts(int productId, String username) {
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
                    .isCompensation(personProduct.isCompensation())
                    .compensationAmount(personProduct.getCompensationAmount())
                    .isSettled(personProduct.isSettled())
                    .build();
            responseList.add(response);
        }
        return ResponseEntity.ok(responseList);
    }

}
