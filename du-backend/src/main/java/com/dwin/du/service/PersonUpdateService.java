package com.dwin.du.service;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person.PersonRepository;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.user.User;
import com.dwin.du.valid.ValidService;
import org.springframework.stereotype.Service;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

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

    public void updateReceiptsCount(String username) {
        User user = valid.validateUser(username);
        List<Person> persons = personRepository.findByUser(user);

        for (Person person : persons) {
            int receiptsCount = 0;
            List<PersonProduct> personProducts = personProductRepository.findByPerson(person);

            Set<Integer> receiptIds = new HashSet<>();
            for (PersonProduct personProduct : personProducts) {
                Product product = personProduct.getProduct();

                if (product.getReceipt() != null) {
                    receiptIds.add(product.getReceipt().getId());
                }
            }

            receiptsCount = receiptIds.size();
            person.setReceiptsCount(receiptsCount);
            personRepository.save(person);
        }
    }


    public void updateTotalAmount(String username){
        User user = valid.validateUser(username);
        List<Person> persons = personRepository.findByUser(user);

        for (Person person : persons) {
            double totalAmount = personProductRepository.findByPerson(person)
                    .stream()
                    .mapToDouble(PersonProduct::getPartOfPrice)
                    .sum();
            person.setTotalAmount(totalAmount);
            personRepository.save(person);
        }
    }

    public void updateUnpaidAmount(String username){
        User user = valid.validateUser(username);
        List<Person> persons = personRepository.findByUser(user);

        for (Person person : persons) {
            double unpaidAmount = personProductRepository.findByPerson(person)
                    .stream()
                    .filter(personProduct -> !personProduct.isSettled())
                    .mapToDouble(PersonProduct::getPartOfPrice)
                    .sum();
            person.setUnpaidAmount(unpaidAmount);
            personRepository.save(person);
        }
    }

    public void updateAmounts(String username){
        updateTotalAmount(username);
        updateUnpaidAmount(username);
    }

    public void updateAllData(String username){
        updateReceiptsCount(username);
        updateTotalAmount(username);
        updateUnpaidAmount(username);
    }
}

