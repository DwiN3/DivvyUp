package com.dwin.du.validation;
import com.dwin.du.api.entity.*;
import com.dwin.du.api.repository.*;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class ValidationService {
    private final UserRepository userRepository;
    private final PersonRepository personRepository;
    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;
    private final LoanRepository loanRepository;

    public User validateUser(String username) throws ValidationException {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            throw new ValidationException(HttpStatus.NOT_FOUND, "Nie znaleziono użytkownika: " + username);

        return optionalUser.get();
    }

    public Person validatePerson(String username, int personId) throws ValidationException {
        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            throw new ValidationException(HttpStatus.NOT_FOUND, "Nie znaleziono osobę o id: " + personId);

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            throw new ValidationException(HttpStatus.UNAUTHORIZED, "Brak dostępu do osoby o id: " + personId);

        return person;
    }

    public Receipt validateReceipt(String username, int receiptId) throws ValidationException {
        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            throw new ValidationException(HttpStatus.NOT_FOUND, "Nie znaleziono rachunku o id: " + receiptId);

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            throw new ValidationException(HttpStatus.UNAUTHORIZED, "Brak dostępu do rachunku o id: " + receiptId);

        return receipt;
    }

    public Product validateProduct(String username, int productId) throws ValidationException {
        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent())
            throw new ValidationException(HttpStatus.NOT_FOUND, "Nie znaleziono produktu o id: " + productId);

        Product product = optionalProduct.get();
        if (!product.getUser().getUsername().equals(username))
            throw new ValidationException(HttpStatus.UNAUTHORIZED, "Brak dostępu do produktu o id: " + productId);

        return product;
    }

    public PersonProduct validatePersonProduct(String username, int personProductId) throws ValidationException {
        Optional<PersonProduct> optionalPersonProduct = personProductRepository.findById(personProductId);
        if (!optionalPersonProduct.isPresent())
            throw new ValidationException(HttpStatus.NOT_FOUND, "Nie znaleziono przypisu osoby do produktu o id: " + personProductId);

        PersonProduct personProduct = optionalPersonProduct.get();
        if (!personProduct.getUser().getUsername().equals(username))
            throw new ValidationException(HttpStatus.UNAUTHORIZED, "Brak dostępu do przypisu osoby do produktu o id: " + personProductId);

        return personProduct;
    }

    public Loan validateLoan(String username, int loanId) throws ValidationException {
        Optional<Loan> optionalLoan = loanRepository.findById(loanId);
        if (!optionalLoan.isPresent())
            throw new ValidationException(HttpStatus.NOT_FOUND, "Nie znaleziono pożyczki o id: " + loanId);

        Loan loan = optionalLoan.get();
        if (!loan.getUser().getUsername().equals(username))
            throw new ValidationException(HttpStatus.UNAUTHORIZED, "Brak dostępu do pożyczki o id: " + loanId);

        return loan;
    }

    public void isNull(Object object, String message){
        if(object == null)
            throw new ValidationException(HttpStatus.BAD_REQUEST, message);
    }

    public void isEmpty(String string, String message){
        if(string == null || string.isEmpty())
            throw new ValidationException(HttpStatus.BAD_REQUEST, message);
    }
}
