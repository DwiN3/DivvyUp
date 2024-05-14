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

    public ResponseEntity<?> addPerson(AddPersonRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Person person = Person.builder()
                    .user(user)
                    .name(request.getName())
                    .surname(request.getSurname())
                    .receiptsCount(0)
                    .totalPurchaseAmount(0.0)
                    .build();
            personRepository.save(person);
            return ResponseEntity.ok().build();
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> editPerson(int personId, AddPersonRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Optional<Person> optionalPerson = personRepository.findById(personId);
            if (optionalPerson.isPresent()) {
                Person person = optionalPerson.get();
                if (person.getUser().getUsername().equals(username)) {
                    person.setName(request.getName());
                    person.setSurname(request.getSurname());
                    personRepository.save(person);
                    return ResponseEntity.ok().build();
                } else {
                    return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
                }
            } else {
                return ResponseEntity.notFound().build();
            }
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> removePerson(int personId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Optional<Person> optionalPerson = personRepository.findById(personId);
            if (optionalPerson.isPresent()) {
                Person person = optionalPerson.get();
                if (person.getUser().getUsername().equals(username)) {
                    personRepository.delete(person);
                    return ResponseEntity.ok().build();
                } else {
                    return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
                }
            } else {
                return ResponseEntity.notFound().build();
            }
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> setReceiptsCounts(int personId, SetPersonReceiptsCountsRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Optional<Person> optionalPerson = personRepository.findById(personId);
            if (optionalPerson.isPresent()) {
                Person person = optionalPerson.get();
                if (person.getUser().getUsername().equals(username)) {
                    person.setReceiptsCount(request.getReceiptsCount());
                    personRepository.save(person);
                    return ResponseEntity.ok().build();
                } else {
                    return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
                }
            } else {
                return ResponseEntity.notFound().build();
            }
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> setTotalPurchaseAmount(int personId, SetTotalAmountReceiptRequest request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Optional<Person> optionalPerson = personRepository.findById(personId);
            if (optionalPerson.isPresent()) {
                Person person = optionalPerson.get();
                if (person.getUser().getUsername().equals(username)) {
                    person.setTotalPurchaseAmount(request.getTotalPurchaseAmount());
                    personRepository.save(person);
                    return ResponseEntity.ok().build();
                } else {
                    return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
                }
            } else {
                return ResponseEntity.notFound().build();
            }
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> showPersonById(int personId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            Optional<Person> optionalPerson = personRepository.findById(personId);
            if (optionalPerson.isPresent()) {
                Person person = optionalPerson.get();
                if (person.getUser().getUsername().equals(username)) {
                    ShowPersonResponse response = ShowPersonResponse.builder()
                            .personId(person.getPersonId())
                            .name(person.getName())
                            .surname(person.getSurname())
                            .receiptsCount(person.getReceiptsCount())
                            .totalPurchaseAmount(person.getTotalPurchaseAmount())
                            .build();
                    return ResponseEntity.ok(response);
                } else {
                    return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
                }
            } else {
                return ResponseEntity.notFound().build();
            }
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> showPersons(String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (optionalUser.isPresent()) {
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
        } else {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }
}