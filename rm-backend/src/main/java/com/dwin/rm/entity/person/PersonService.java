package com.dwin.rm.entity.person;

import com.dwin.rm.entity.person.Request.AddPersonRequest;
import com.dwin.rm.entity.person.Request.SetPersonReceiptsCountsRequest;
import com.dwin.rm.entity.person.Request.SetTotalAmountReceiptRequest;
import com.dwin.rm.entity.person.Response.ShowPersonResponse;
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
public class PersonService {

    private final PersonRepository personRepository;
    private final UserRepository userRepository;

    private ResponseEntity<?> checkUser(String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        return null;
    }

    public ResponseEntity<?> addPerson(AddPersonRequest request, String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

        User user = userRepository.findByUsername(username).get();
        Person person = Person.builder()
                .user(user)
                .name(request.getName())
                .surname(request.getSurname())
                .receiptsCount(0)
                .totalPurchaseAmount(0.0)
                .build();
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> editPerson(int personId, AddPersonRequest request, String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

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
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        personRepository.delete(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setReceiptsCounts(int personId, SetPersonReceiptsCountsRequest request, String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

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


    public ResponseEntity<?> setTotalPurchaseAmount(int personId, SetTotalAmountReceiptRequest request, String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        person.setTotalPurchaseAmount(request.getTotalPurchaseAmount());
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> showPersonById(int personId, String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

        Optional<Person> optionalPerson = personRepository.findById(personId);
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        ShowPersonResponse response = ShowPersonResponse.builder()
                .personId(person.getPersonId())
                .name(person.getName())
                .surname(person.getSurname())
                .receiptsCount(person.getReceiptsCount())
                .totalPurchaseAmount(person.getTotalPurchaseAmount())
                .build();
        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> showPersons(String username) {
        ResponseEntity<?> userCheckResponse = checkUser(username);
        if (userCheckResponse != null)
            return userCheckResponse;

        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        User user = optionalUser.get();
        List<Person> persons = personRepository.findByUser(user);
        List<ShowPersonResponse> responseList = new ArrayList<>();
        for (Person person : persons) {
            ShowPersonResponse response = ShowPersonResponse.builder()
                    .personId(person.getPersonId())
                    .name(person.getName())
                    .surname(person.getSurname())
                    .receiptsCount(person.getReceiptsCount())
                    .totalPurchaseAmount(person.getTotalPurchaseAmount())
                    .build();
            responseList.add(response);
        }
        return ResponseEntity.ok(responseList);
    }
}