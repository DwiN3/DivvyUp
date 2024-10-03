package com.dwin.du.entity.person;

import com.dwin.du.entity.person.Request.AddPersonRequest;
import com.dwin.du.entity.person.Request.SetPersonReceiptsCountsRequest;
import com.dwin.du.entity.person.Request.SetTotalAmountReceiptRequest;
import com.dwin.du.entity.user.User;
import com.dwin.du.entity.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class PersonService {

    private final UserRepository userRepository;
    private final PersonRepository personRepository;

    public ResponseEntity<?> addPerson(AddPersonRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        User user = userRepository.findByUsername(username).get();
        Person person = Person.builder()
                .user(user)
                .name(request.getName())
                .surname(request.getSurname())
                .receiptsCount(0)
                .totalAmount(0.0)
                .unpaidAmount(0.0)
                .build();
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> editPerson(int personId, AddPersonRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        person.setName(request.getName());
        person.setSurname(request.getSurname());
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removePerson(int personId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        // TO DO DO poprawki
        /*
        List<Receipt> receipts = receiptRepository.findByUser(person.getUser());
        List<PersonProduct> personProducts = personProductRepository.findByPerson(person);
        if (!receipts.isEmpty() || !personProducts.isEmpty()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }
        */

        personRepository.delete(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setReceiptsCounts(int personId, SetPersonReceiptsCountsRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        person.setReceiptsCount(request.getReceiptsCount());
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> setTotalAmount(int personId, SetTotalAmountReceiptRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        person.setTotalAmount(request.getTotalAmount());
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getPersonById(int personId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        PersonDto response = PersonDto.builder()
                .id(person.getId())
                .name(person.getName())
                .surname(person.getSurname())
                .receiptsCount(person.getReceiptsCount())
                .totalAmount(person.getTotalAmount())
                .unpaidAmount(person.getUnpaidAmount())
                .build();

        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getPersons(String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        User user = optionalUser.get();
        List<Person> persons = personRepository.findByUser(user);
        List<PersonDto> responseList = new ArrayList<>();
        for (Person person : persons) {
            PersonDto response = PersonDto.builder()
                    .id(person.getId())
                    .name(person.getName())
                    .surname(person.getSurname())
                    .receiptsCount(person.getReceiptsCount())
                    .totalAmount(person.getTotalAmount())
                    .build();
            responseList.add(response);

        }
        return ResponseEntity.ok(responseList);
    }
}