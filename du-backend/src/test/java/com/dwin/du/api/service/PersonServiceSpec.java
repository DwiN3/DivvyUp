package com.dwin.du.api.service;

import com.dwin.du.api.dto.PersonDto;
import com.dwin.du.api.entity.Person;
import com.dwin.du.api.entity.User;
import com.dwin.du.api.repository.*;
import com.dwin.du.api.request.AddEditPersonRequest;
import com.dwin.du.validation.ValidationException;
import com.dwin.du.validation.ValidationService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import java.util.Optional;
import static org.assertj.core.api.Assertions.assertThat;
import static org.mockito.BDDMockito.*;

public class PersonServiceSpec {

    @Mock
    private UserRepository userRepository;

    @Mock
    private PersonRepository personRepository;

    @Mock
    private ValidationService validator;

    @InjectMocks
    private PersonService personService;

    private User mockUser;

    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
        mockUser = new User();
        mockUser.setUsername("testuser");

        given(userRepository.findByUsername("testuser")).willReturn(Optional.of(mockUser));
    }


    @Test
    void shouldAddPersonSuccessfully() {
        // Given
        given(validator.validateUser("testuser")).willReturn(mockUser);
        AddEditPersonRequest request = new AddEditPersonRequest("Robert", "Lewandowski");

        // When
        ResponseEntity<?> response = personService.addPerson("testuser", request);

        // Then
        assertThat(response.getStatusCode()).isEqualTo(HttpStatus.OK);
        then(personRepository).should(times(1)).save(any(Person.class));
    }

    @Test
    void shouldAddPersonReturnBadRequestEmptyName() {
        // Given
        given(validator.validateUser("testuser")).willReturn(mockUser);
        AddEditPersonRequest request = new AddEditPersonRequest("", "Lewandowski");

        // When
        ResponseEntity<?> response = personService.addPerson("testuser", request);

        // Then
        assertThat(response.getStatusCode()).isEqualTo(HttpStatus.BAD_REQUEST);
        assertThat(response.getBody()).isEqualTo("Nazwa osoby jest wymagana");
    }

    @Test
    void shouldGetPersonSuccessfully() throws ValidationException {
        // Given
        String username = "testuser";
        int personId = 1;

        Person mockPerson = new Person();
        mockPerson.setId(personId);
        mockPerson.setName("Robert");
        mockPerson.setSurname("Lewandowski");
        mockPerson.setReceiptsCount(5);
        mockPerson.setProductsCount(10);
        mockPerson.setTotalAmount(100.0);
        mockPerson.setUnpaidAmount(20.0);
        mockPerson.setUserAccount(false);

        given(validator.validateUser(username)).willReturn(mockUser);
        given(validator.validatePerson(username, personId)).willReturn(mockPerson);

        // When
        ResponseEntity<?> response = personService.getPerson(username, personId);

        // Then
        assertThat(response.getStatusCode()).isEqualTo(HttpStatus.OK);
        assertThat(response.getBody()).isInstanceOf(PersonDto.class);

        PersonDto personDto = (PersonDto) response.getBody();
        assertThat(personDto.getId()).isEqualTo(mockPerson.getId());
        assertThat(personDto.getName()).isEqualTo(mockPerson.getName());
        assertThat(personDto.getSurname()).isEqualTo(mockPerson.getSurname());
        assertThat(personDto.getReceiptsCount()).isEqualTo(mockPerson.getReceiptsCount());
        assertThat(personDto.getProductsCount()).isEqualTo(mockPerson.getProductsCount());
        assertThat(personDto.getTotalAmount()).isEqualTo(mockPerson.getTotalAmount());
        assertThat(personDto.getUnpaidAmount()).isEqualTo(mockPerson.getUnpaidAmount());
        assertThat(personDto.isUserAccount()).isEqualTo(mockPerson.isUserAccount());
    }

    @Test
    void shouldReturnNotFoundWhenPersonDoesNotExist() throws ValidationException {
        // Given
        String username = "testuser";
        int personId = 0;

        given(validator.validateUser(username)).willReturn(mockUser);
        given(validator.validatePerson(username, personId)).willThrow(new ValidationException(HttpStatus.NOT_FOUND, "Nie znaleziono osobę o id: " + personId));

        // When
        ResponseEntity<?> response = personService.getPerson(username, personId);

        // Then
        assertThat(response.getStatusCode()).isEqualTo(HttpStatus.NOT_FOUND);
        assertThat(response.getBody()).isEqualTo("Nie znaleziono osobę o id: " + personId);
    }
}
