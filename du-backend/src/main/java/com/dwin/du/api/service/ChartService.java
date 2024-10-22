package com.dwin.du.api.service;
import com.dwin.du.api.dto.ChartDto;
import com.dwin.du.api.entity.Person;
import com.dwin.du.api.entity.Receipt;
import com.dwin.du.api.repository.PersonRepository;
import com.dwin.du.api.entity.User;
import com.dwin.du.api.repository.ReceiptRepository;
import com.dwin.du.validation.ValidationException;
import com.dwin.du.validation.ValidationService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.time.LocalDate;
import java.time.ZoneId;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
@RequiredArgsConstructor
public class ChartService {

    private final PersonRepository personRepository;
    private final ReceiptRepository receiptRepository;
    private final ValidationService validator;

    private static final String[] MONTH_NAMES = {"Sty", "Lut", "Mar", "Kwi", "Maj", "Cze", "Lip", "Sie", "Wrz", "Paź", "Lis", "Gru"};

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

    public ResponseEntity<?> getMonthlyTotalExpenses(String username, int year) {
        try {
            User user = validator.validateUser(username);

            List<Receipt> receipts = receiptRepository.findByUser(user);
            Map<Integer, Double> monthlyTotals = new HashMap<>();

            for (Receipt receipt : receipts) {
                LocalDate date = receipt.getDate().toInstant().atZone(ZoneId.systemDefault()).toLocalDate();

                if (date.getYear() == year) {
                    int month = date.getMonthValue();
                    monthlyTotals.put(month, monthlyTotals.getOrDefault(month, 0.0) + receipt.getTotalPrice());
                }
            }

            List<ChartDto> responseList = new ArrayList<>();
            for (int month = 1; month <= 12; month++) {
                double total = monthlyTotals.getOrDefault(month, 0.0);
                responseList.add(ChartDto.builder()
                        .name(MONTH_NAMES[month - 1])
                        .value(total)
                        .build());
            }

            return ResponseEntity.ok(responseList);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    private double calculatePaidPercentage(double totalAmount, double unpaidAmount) {
        if (totalAmount == 0) return 100;
        if (unpaidAmount == 0) return 100;
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
