package com.dwin.du.service;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person.PersonRepository;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.user.User;
import com.dwin.du.valid.ValidService;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class PersonUpdateService {
    private final PersonRepository personRepository;
    private final PersonProductRepository personProductRepository;
    private final ValidService valid;

    public PersonUpdateService(PersonRepository personRepository, PersonProductRepository personProductRepository, ValidService valid) {
        this.personRepository = personRepository;
        this.personProductRepository = personProductRepository;
        this.valid = valid;
    }

    public void updateAllData(String username) {
        User user = valid.validateUser(username);
        List<Person> persons = personRepository.findByUser(user);

        for (Person person : persons) {
            List<PersonProduct> personProducts = personProductRepository.findByPerson(person);

            long receiptsCount = personProducts.stream()
                    .map(personProduct -> personProduct.getProduct())
                    .filter(product -> product.getReceipt() != null)
                    .map(product -> product.getReceipt().getId())
                    .distinct()
                    .count();

            person.setReceiptsCount((int) receiptsCount);

            person.setProductsCount(personProducts.size());

            double totalAmount = personProducts.stream()
                    .mapToDouble(PersonProduct::getPartOfPrice)
                    .sum();
            person.setTotalAmount(totalAmount);

            double unpaidAmount = personProducts.stream()
                    .filter(personProduct -> !personProduct.isSettled())
                    .mapToDouble(PersonProduct::getPartOfPrice)
                    .sum();
            person.setUnpaidAmount(unpaidAmount);
        }

        personRepository.saveAll(persons);
    }
}

