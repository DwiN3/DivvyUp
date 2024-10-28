package com.dwin.du.api.service;
import com.dwin.du.api.dto.LoanDto;
import com.dwin.du.api.entity.*;
import com.dwin.du.api.repository.LoanRepository;
import com.dwin.du.api.repository.PersonRepository;
import com.dwin.du.api.request.AddEditLoanRequest;
import com.dwin.du.update.EntityUpdateService;
import com.dwin.du.validation.ValidationException;
import com.dwin.du.validation.ValidationService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class LoanService {
    private final LoanRepository loanRepository;
    private final PersonRepository personRepository;
    private final EntityUpdateService updater;
    private final ValidationService validator;

    public ResponseEntity<?> addLoan(String username, AddEditLoanRequest request) {
        try {
            User user = validator.validateUser(username);
            validator.isNull(request, "Nie przekazano danych");
            validator.isNull(request.getPersonId(), "Osoba jest wymagana");
            validator.isNull(request.getAmount(), "Ilość jest wymagana");
            validator.isNull(request.getDate(), "Data jest wymagana");
            Person person = validator.validatePerson(username, request.getPersonId());

            Loan loan = Loan.builder()
                    .person(person)
                    .date(request.getDate())
                    .amount(request.getAmount())
                    .lent(request.isLent())
                    .settled(false)
                    .build();

            loanRepository.save(loan);
            updater.updatePerson(username, true);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> editLoan(String username, int loanId, AddEditLoanRequest request) {
        try {
            validator.validateUser(username);
            validator.isNull(request, "Nie przekazano danych");
            validator.isNull(request.getAmount(), "Ilość jest wymagana");
            validator.isNull(request.getDate(), "Data jest wymagana");
            validator.isNull(loanId, "Pusty id pożyczki");
            Loan loan = validator.validateLoan(username, loanId);

            loan.setDate(request.getDate());
            loan.setAmount(request.getAmount());

            loanRepository.save(loan);
            updater.updatePerson(username, true);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> removeLoan(String username, int loanId) {
        try {
            validator.validateUser(username);
            validator.isNull(loanId, "Brak identyfikatora pożyczki");
            Loan loan = validator.validateLoan(username, loanId);

            loanRepository.delete(loan);
            updater.updatePerson(username, true);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> setPerson(String username, int loanId, int personId) {
        try {
            validator.validateUser(username);
            validator.isNull(loanId, "Brak identyfikatora pożyczki");
            validator.isNull(personId, "Brak identyfikatora osoby");
            Person person = validator.validatePerson(username, personId);
            Loan loan = validator.validateLoan(username, loanId);

            loan.setPerson(person);

            loanRepository.save(loan);
            updater.updatePerson(username, true);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> setIsSettled(String username, int loanId, boolean settled) {
        try {
            validator.validateUser(username);
            validator.isNull(loanId, "Brak identyfikatora pożyczki");
            Loan loan = validator.validateLoan(username, loanId);

            loan.setSettled(settled);

            loanRepository.save(loan);
            updater.updatePerson(username, true);
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> setIsLent(String username, int loanId, boolean lent) {
        try {
            validator.validateUser(username);
            validator.isNull(loanId, "Brak identyfikatora pożyczki");
            Loan loan = validator.validateLoan(username, loanId);

            loan.setLent(lent);

            loanRepository.save(loan);
            updater.updatePerson(username, true );
            return ResponseEntity.ok().build();

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getLoan(String username, int loanId) {
        try {
            validator.validateUser(username);
            validator.isNull(loanId, "Brak identyfikatora pożyczki");
            Loan loan = validator.validateLoan(username, loanId);

            LoanDto response = LoanDto.builder()
                    .id(loan.getId())
                    .personId(loan.getPerson().getId())
                    .date(loan.getDate())
                    .amount(loan.getAmount())
                    .settled(loan.isSettled())
                    .lent(loan.isLent())
                    .build();

            return ResponseEntity.ok(response);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getLoanPerson(String username, int personId) {
        try {
            validator.validateUser(username);
            validator.isNull(personId, "Brak identyfikatora osoby");
            Person person = validator.validatePerson(username,personId);

            List<Loan> loans = loanRepository.findByPerson(person);
            List<LoanDto> responseList = new ArrayList<>();
            for (Loan loan : loans) {
                LoanDto response = LoanDto.builder()
                        .id(loan.getId())
                        .personId(loan.getPerson().getId())
                        .date(loan.getDate())
                        .amount(loan.getAmount())
                        .settled(loan.isSettled())
                        .lent(loan.isLent())
                        .build();
                responseList.add(response);
            }

            return ResponseEntity.ok(responseList);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getLoans(String username) {
        try {
            User user = validator.validateUser(username);

            List<Person> persons = personRepository.findByUser(user);
            List<Loan> loans = loanRepository.findByPersonIn(persons);
            List<LoanDto> responseList = new ArrayList<>();
            for (Loan loan : loans) {
                LoanDto response = LoanDto.builder()
                        .id(loan.getId())
                        .personId(loan.getPerson().getId())
                        .date(loan.getDate())
                        .amount(loan.getAmount())
                        .settled(loan.isSettled())
                        .lent(loan.isLent())
                        .build();
                responseList.add(response);
            }

            return ResponseEntity.ok(responseList);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getLoansByDataRange(String username, String fromDate, String toDate) {
        try {
            User user = validator.validateUser(username);
            validator.isEmpty(fromDate, "Data początkowa jest wymagana");
            validator.isEmpty(toDate, "Data końcowa jest wymagana");

            SimpleDateFormat dateFormat = new SimpleDateFormat("dd-MM-yyyy");
            Date dateFrom = dateFormat.parse(fromDate);
            Date dateTo = dateFormat.parse(toDate);
            
            dateTo = new Date(dateTo.getTime());
            dateTo.setHours(23);
            dateTo.setMinutes(59);
            dateTo.setSeconds(59);

            List<Person> persons = personRepository.findByUser(user);
            List<Loan> loans = loanRepository.findByPersonInAndDateBetween(persons, dateFrom, dateTo);

            List<LoanDto> responseList = loans.stream()
                    .map(loan -> LoanDto.builder()
                            .id(loan.getId())
                            .personId(loan.getPerson().getId())
                            .date(loan.getDate())
                            .amount(loan.getAmount())
                            .settled(loan.isSettled())
                            .lent(loan.isLent())
                            .build())
                    .collect(Collectors.toList());

            return ResponseEntity.ok(responseList);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    private ResponseEntity<?> handleException(Exception e) {
        if (e instanceof ValidationException) {
            HttpStatus status = ((ValidationException) e).getStatus();
            return ResponseEntity.status(status).body(e.getMessage());
        }
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Wystąpił nieoczekiwany błąd.");
    }
}
