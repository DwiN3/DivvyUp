package com.dwin.rm.entity.person;

import com.dwin.rm.entity.person.Response.AddPersonResponse;
import com.dwin.rm.entity.person.Request.AddPersonRequest;
import com.dwin.rm.security.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class PersonService {

    /*private final PersonRepository personRepository;
    private final UserRepository userRepository;

    public ResponseEntity<?> addPerson(AddPersonRequest request, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var person = Person.builder()
                .addedByUserId(user.getUserId())
                .name(request.getName())
                .surname(request.getSurname())
                .build();

        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> editPerson(int personId, AddPersonRequest request, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var existingPerson = personRepository.findById(personId).orElse(null);
        if (existingPerson == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
        if (existingPerson.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }

        existingPerson.setName(request.getName());
        existingPerson.setSurname(request.getSurname());
        personRepository.save(existingPerson);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> removePerson(int personId, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var person = personRepository.findById(personId).orElse(null);
        if (person == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
        if (person.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }
        personRepository.delete(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> showPersonById(int personId, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var person = personRepository.findById(personId).orElse(null);
        if (person == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }

        return ResponseEntity.ok(person);
    }

    public ResponseEntity<?> showPersons(String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var persons = personRepository.findAllByAddedByUserId(user.getUserId());
        return ResponseEntity.ok(persons);
    }

    public ResponseEntity<?> setReceiptsCounts(int personId, int receiptsCount, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var person = personRepository.findById(personId).orElse(null);
        if (person == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
        if (person.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }
        person.setReceiptsCount(receiptsCount);
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setTotalPurchaseAmount(int personId, double totalPurchaseAmount, String currentUsername) {
        var user = userRepository.findByUsername(currentUsername).orElse(null);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
        var person = personRepository.findById(personId).orElse(null);
        if (person == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
        if (person.getAddedByUserId() != user.getUserId()) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).build();
        }
        person.setTotalPurchaseAmount(totalPurchaseAmount);
        personRepository.save(person);
        return ResponseEntity.ok().build();
    }*/
}