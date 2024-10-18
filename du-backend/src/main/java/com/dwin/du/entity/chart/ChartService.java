package com.dwin.du.entity.chart;
import com.dwin.du.entity.chart.Response.ChartDto;
import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person.PersonRepository;
import com.dwin.du.entity.user.User;
import com.dwin.du.validation.ValidationService;
import lombok.RequiredArgsConstructor;
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
    }

    public ResponseEntity<?> getPercentageExpanses(String username) {
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
    }

    private double calculatePaidPercentage(double totalAmount, double unpaidAmount) {
        if (totalAmount == 0) return 0;
        double percentage = (1 - (unpaidAmount / totalAmount)) * 100;
        return percentage;
        //return Math.round(percentage * 100.0) / 100.0;
    }
}
