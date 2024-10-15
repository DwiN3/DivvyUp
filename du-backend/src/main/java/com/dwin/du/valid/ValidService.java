package com.dwin.du.valid;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person.PersonRepository;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.receipt.ReceiptRepository;
import com.dwin.du.entity.user.User;
import com.dwin.du.entity.user.UserRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class ValidService {
    private final UserRepository userRepository;
    private final PersonRepository personRepository;
    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;

    public ValidService(UserRepository userRepository, PersonRepository personRepository, ReceiptRepository receiptRepository, ProductRepository productRepository, PersonProductRepository personProductRepository) {
        this.userRepository = userRepository;
        this.personRepository = personRepository;
        this.receiptRepository = receiptRepository;
        this.productRepository = productRepository;
        this.personProductRepository = personProductRepository;
    }

    public User validateUser(String username) throws ValidException {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            throw new ValidException(401, "Brak dostępu do użytkownika: " + username);
        }

        return optionalUser.get();
    }

    public Person validatePerson(String userId, int personId) throws ValidException {
        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent()) {
            throw new ValidException(404, "Nie znaleziono osobę o id: " + personId);
        }

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(userId)) {
            throw new ValidException(401, "Brak dostępu do osoby o id: " + personId);
        }

        return person;
    }

    public Receipt validateReceipt(String userId, int receiptId) throws ValidException {
        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent()) {
            throw new ValidException(404, "Nie znaleziono rachunku o id: " + receiptId);
        }

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(userId)) {
            throw new ValidException(401, "Brak dostępu do rachunku o id: " + receiptId);
        }

        return receipt;
    }

    public Product validateProduct(String userId, int productId) throws ValidException {
        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent()) {
            throw new ValidException(404, "Nie znaleziono produktu o id: " + productId);
        }

        Product product = optionalProduct.get();
        if (!product.getUser().getUsername().equals(userId)) {
            throw new ValidException(401, "Brak dostępu do produktu o id: " + productId);
        }

        return product;
    }

    public PersonProduct validatePersonProduct(String userId, int personProductId) throws ValidException {
        Optional<PersonProduct> optionalPersonProduct = personProductRepository.findById(personProductId);
        if (!optionalPersonProduct.isPresent()) {
            throw new ValidException(404, "Nie znaleziono przypisu osoby do produktu o id: " + personProductId);
        }

        PersonProduct personProduct = optionalPersonProduct.get();
        if (!personProduct.getUser().getUsername().equals(userId)) {
            throw new ValidException(401, "Brak dostępu do przypisu osoby do produktu o id: " + personProductId);
        }

        return personProduct;
    }

    public void isNull(Object object){
        if(object == null)
            throw new ValidException(400, "Puste dane");
    }

    public void isEmpty(String string){
        if(string.isEmpty())
            throw new ValidException(400, "Puste dane");
    }
}
