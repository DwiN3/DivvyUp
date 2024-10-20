package com.dwin.du.api.service;
import com.dwin.du.api.dto.UserDto;
import com.dwin.du.api.entity.*;
import com.dwin.du.api.repository.*;
import com.dwin.du.config.JwtService;
import com.dwin.du.api.request.EditUserRequest;
import com.dwin.du.api.request.LoginRequest;
import com.dwin.du.api.request.PasswordChangeRequest;
import com.dwin.du.api.request.RegisterRequest;
import com.dwin.du.api.model.Role;
import com.dwin.du.validation.ValidationException;
import com.dwin.du.validation.ValidationService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class UserService {

    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final JwtService jwtService;
    private final AuthenticationManager authenticationManager;
    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;
    private final PersonRepository personRepository;
    private final ValidationService validator;

    public ResponseEntity<?> authenticate(LoginRequest request) {
        try {
            validator.isNull(request, "Nie przekazano danych");
            validator.isEmpty(request.getUsername(), "Nazwa użytkownika jest wymagana");
            validator.isEmpty(request.getPassword(), "Hasło jest wymagane");
            User user = validator.validateUser(request.getUsername());

            if (!passwordEncoder.matches(request.getPassword(), user.getPassword()))
                return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

            Authentication authentication = authenticationManager.authenticate(
                    new UsernamePasswordAuthenticationToken(
                            request.getUsername(),
                            request.getPassword()
                    )
            );

            SecurityContextHolder.getContext().setAuthentication(authentication);
            var jwtToken = jwtService.generateTokem(user);

            return ResponseEntity.ok(jwtToken);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> register(RegisterRequest request) {
        try {
            validator.isNull(request, "Nie przekazano danych");
            validator.isEmpty(request.getUsername(), "Nazwa użytkownika jest wymagana");
            validator.isEmpty(request.getEmail(), "Mail użytkownika jest wymagany");
            validator.isEmpty(request.getPassword(), "Hasło jest wymagane");

            if (userRepository.findByEmail(request.getEmail()).isPresent())
                return ResponseEntity.status(HttpStatus.CONFLICT).build();

            else if (userRepository.findByUsername(request.getUsername()).isPresent())
                return ResponseEntity.status(HttpStatus.CONFLICT).build();

            var user = User.builder()
                    .username(request.getUsername())
                    .email(request.getEmail())
                    .password(passwordEncoder.encode(request.getPassword()))
                    .role(Role.USER)
                    .build();

            userRepository.save(user);
            AddPersonForUser(user);

            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> editUser(String username, EditUserRequest request) {
        try {
            validator.isNull(request, "Nie przekazano danych");
            validator.isEmpty(request.getUsername(), "Nazwa użytkownika jest wymagana");
            validator.isEmpty(request.getEmail(), "Mail użytkownika jest wymagany");
            User user = validator.validateUser(username);

            user.setUsername(request.getUsername());
            user.setEmail(request.getEmail());
            userRepository.save(user);
            String newToken = jwtService.generateTokem(user);

            return ResponseEntity.ok(newToken);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> removeUser(String username) {
        try {
            User user = validator.validateUser(username);

            List<PersonProduct> personProducts = personProductRepository.findByUser(user);
            personProductRepository.deleteInBatch(personProducts);
            List<Product> products = productRepository.findByUser(user);
            productRepository.deleteInBatch(products);
            List<Receipt> receipts = receiptRepository.findByUser(user);
            receiptRepository.deleteInBatch(receipts);
            List<Person> persons = personRepository.findByUser(user);
            personRepository.deleteInBatch(persons);

            userRepository.delete(user);
            SecurityContextHolder.clearContext();
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> validateToken(String token) {
        try {
            validator.isEmpty(token, "Nie przekazano tokena");
            String username = jwtService.extractUsername(token);
            UserDetails userDetails = userRepository.findByUsername(username).orElseThrow(() -> new UsernameNotFoundException("User not found"));
            boolean isValid = jwtService.isTokenValid(token, userDetails);

            if (isValid)
                return ResponseEntity.ok(true);
            else
                return ResponseEntity.ok(false);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getUser(String token) {
        try {
            validator.isEmpty(token, "Nie przekazano tokena");
            String username = jwtService.extractUsername(token);
            User user = validator.validateUser(username);
            boolean isValid = jwtService.isTokenValid(token, user);

            if(isValid){
                UserDto response = UserDto.builder()
                        .id(user.getId())
                        .username(user.getUsername())
                        .email(user.getEmail())
                        .password(user.getPassword())
                        .build();
                return ResponseEntity.ok(response);
            }
            else
                throw new ValidationException(HttpStatus.UNAUTHORIZED, "Brak dostępu do użytkownika");

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> changePassword(String username, PasswordChangeRequest request) {
        try {
            User user = validator.validateUser(username);
            validator.isNull(request, "Nie przekazano danych");
            validator.isEmpty(request.getPassword(), "Hasło jest wymagane");
            validator.isEmpty(request.getNewPassword(), "Nowe hasło jest wymagane");

            if (!passwordEncoder.matches(request.getPassword(), user.getPassword()))
                return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body("Aktualne hasło jest błędne");

            String newPasswordEncoded = passwordEncoder.encode(request.getNewPassword());

            user.setPassword(newPasswordEncoded);

            userRepository.save(user);
            return ResponseEntity.ok("Hasło zostało zmienione");

        } catch (Exception e) {
            return handleException(e);
        }
    }

    private void AddPersonForUser(User user){
        Person person = Person.builder()
                .user(user)
                .name(user.getUsername())
                .surname("")
                .receiptsCount(0)
                .productsCount(0)
                .totalAmount(0.0)
                .unpaidAmount(0.0)
                .userAccount(true)
                .build();

        personRepository.save(person);
    }

    private ResponseEntity<?> handleException(Exception e) {
        if (e instanceof ValidationException) {
            HttpStatus status = ((ValidationException) e).getStatus();
            return ResponseEntity.status(status).body(e.getMessage());
        }
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Wystąpił nieoczekiwany błąd.");
    }
}
