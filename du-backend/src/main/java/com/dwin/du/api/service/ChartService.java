package com.dwin.du.api.service;
import com.dwin.du.api.dto.ChartDto;
import com.dwin.du.api.entity.Person;
import com.dwin.du.api.repository.PersonRepository;
import com.dwin.du.api.entity.User;
import com.dwin.du.validation.ValidationException;
import com.dwin.du.validation.ValidationService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
public class ChartService {

    private final PersonRepository personRepository;
    private final ValidationService validator;

    public ResponseEntity<?> getChartAmounts(String username, boolean isTotalAmounts) {
        try {
            User user = validator.validateUser(username);
            List<Person> persons = personRepository.findByUser(user);

            List<ChartDto> responseList = new ArrayList<>();
            for (Person person : persons) {
                double amount = isTotalAmounts ? person.getTotalAmount() : person.getUnpaidAmount();
                //amount = Math.round(amount * 100.0) / 100.0;

                ChartDto response = ChartDto.builder()
                        .name(person.getName()+" "+person.getSurname())
                        .value(amount)
                        .build();
                responseList.add(response);
            }
            return ResponseEntity.ok(responseList);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getPercentageExpanses(String username) {
        try {
            User user = validator.validateUser(username);
            List<Person> persons = personRepository.findByUser(user);

            List<ChartDto> responseList = new ArrayList<>();
            for (Person person : persons) {
                double amount = calculatePaidPercentage(person.getTotalAmount(),person.getUnpaidAmount());

                ChartDto response = ChartDto.builder()
                        .name(person.getName()+" "+person.getSurname())
                        .value(amount)
                        .build();
                responseList.add(response);
            }
            return ResponseEntity.ok(responseList);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    private double calculatePaidPercentage(double totalAmount, double unpaidAmount) {
        if (totalAmount == 0) return 0;
        double percentage = (1 - (unpaidAmount / totalAmount)) * 100;
        return percentage;
        //return Math.round(percentage * 100.0) / 100.0;
    }

    private ResponseEntity<?> handleException(Exception e) {
        if (e instanceof ValidationException) {
            HttpStatus status = ((ValidationException) e).getStatus();
            return ResponseEntity.status(status).body(e.getMessage());
        }
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Wystąpił nieoczekiwany błąd.");
    }
}
