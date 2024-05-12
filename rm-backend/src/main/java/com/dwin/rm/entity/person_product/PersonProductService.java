package com.dwin.rm.entity.person_product;

import com.dwin.rm.entity.person_product.Request.AddPersonProductRequest;
import com.dwin.rm.entity.product.ProductRepository;
import com.dwin.rm.security.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class PersonProductService {

    private final PersonProductRepository personProductRepository;
    private final UserRepository userRepository;
    private final ProductRepository productRepository;

    public ResponseEntity<?> addPersonProduct(int productId, AddPersonProductRequest request, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        var productOptional = productRepository.findById(productId);
        if (productOptional.isEmpty()) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }

        var product = productOptional.get();
        if (product.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }

        PersonProduct personProduct = new PersonProduct();
        personProduct.setProductId(productId);
        personProduct.setAddedByUserId(user.getUserId());
        personProduct.setPersonId(request.getPersonId());
        personProduct.setPartOfPrice(request.getPartOfPrice());
        personProduct.setQuantity(request.getQuantity());
        personProduct.setCompensation(request.isCompensation());
        personProduct.setCompensationAmount(request.getCompensationAmount());

        personProductRepository.save(personProduct);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removePersonProductById(int personProductId, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        var personProductOptional = personProductRepository.findById(personProductId);
        if (personProductOptional.isEmpty()) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }

        var personProduct = personProductOptional.get();
        var productOptional = productRepository.findById(personProduct.getProductId());
        if (productOptional.isEmpty()) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }

        var product = productOptional.get();
        if (product.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }

        personProductRepository.delete(personProduct);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> showPersonProducts(int productId, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        var productOptional = productRepository.findById(productId);
        if (productOptional.isEmpty()) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }

        var product = productOptional.get();
        if (product.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }

        List<PersonProduct> persons = personProductRepository.findAllByProductId(productId);
        return ResponseEntity.ok(persons);
    }

    public ResponseEntity<?> showPersonProductsById(int personProductId, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        var personProductOptional = personProductRepository.findById(personProductId);
        if (personProductOptional.isEmpty()) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }

        var personProduct = personProductOptional.get();
        var productOptional = productRepository.findById(personProduct.getProductId());
        if (productOptional.isEmpty()) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }

        var product = productOptional.get();
        if (product.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }

        return ResponseEntity.ok(personProduct);
    }
}
