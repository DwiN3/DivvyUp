package com.dwin.du.validation;
import com.dwin.du.api.entity.Person;
import com.dwin.du.api.repository.PersonRepository;
import com.dwin.du.api.entity.PersonProduct;
import com.dwin.du.api.repository.PersonProductRepository;
import com.dwin.du.api.entity.Product;
import com.dwin.du.api.repository.ProductRepository;
import com.dwin.du.api.entity.Receipt;
import com.dwin.du.api.repository.ReceiptRepository;
import com.dwin.du.api.entity.User;
import com.dwin.du.api.repository.UserRepository;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class ValidationService {
    private final UserRepository userRepository;
    private final PersonRepository personRepository;
    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;

    public ValidationService(UserRepository userRepository, PersonRepository personRepository, ReceiptRepository receiptRepository, ProductRepository productRepository, PersonProductRepository personProductRepository) {
        this.userRepository = userRepository;
        this.personRepository = personRepository;
        this.receiptRepository = receiptRepository;
        this.productRepository = productRepository;
        this.personProductRepository = personProductRepository;
    }

    public User validateUser(String username) throws ValidationException {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            throw new ValidationException(404, "Nie znaleziono użytkownika: " + username);

        return optionalUser.get();
    }

    public Person validatePerson(String userId, int personId) throws ValidationException {
        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            throw new ValidationException(404, "Nie znaleziono osobę o id: " + personId);


        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(userId))
            throw new ValidationException(401, "Brak dostępu do osoby o id: " + personId);

        return person;
    }

    public Receipt validateReceipt(String userId, int receiptId) throws ValidationException {
        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            throw new ValidationException(404, "Nie znaleziono rachunku o id: " + receiptId);

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(userId))
            throw new ValidationException(401, "Brak dostępu do rachunku o id: " + receiptId);

        return receipt;
    }

    public Product validateProduct(String userId, int productId) throws ValidationException {
        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent())
            throw new ValidationException(404, "Nie znaleziono produktu o id: " + productId);

        Product product = optionalProduct.get();
        if (!product.getUser().getUsername().equals(userId))
            throw new ValidationException(401, "Brak dostępu do produktu o id: " + productId);

        return product;
    }

    public PersonProduct validatePersonProduct(String userId, int personProductId) throws ValidationException {
        Optional<PersonProduct> optionalPersonProduct = personProductRepository.findById(personProductId);
        if (!optionalPersonProduct.isPresent())
            throw new ValidationException(404, "Nie znaleziono przypisu osoby do produktu o id: " + personProductId);

        PersonProduct personProduct = optionalPersonProduct.get();
        if (!personProduct.getUser().getUsername().equals(userId))
            throw new ValidationException(401, "Brak dostępu do przypisu osoby do produktu o id: " + personProductId);

        return personProduct;
    }

    public void isNull(Object object){
        if(object == null)
            throw new ValidationException(400, "Brak danych");
    }

    public void isEmpty(String string){
        if(string.isEmpty())
            throw new ValidationException(400, "Brak danych");
    }
}
