package com.dwin.du.api.service;
import com.dwin.du.api.dto.ChartDto;
import com.dwin.du.api.entity.Person;
import com.dwin.du.api.entity.PersonProduct;
import com.dwin.du.api.entity.Receipt;
import com.dwin.du.api.repository.PersonProductRepository;
import com.dwin.du.api.repository.PersonRepository;
import com.dwin.du.api.entity.User;
import com.dwin.du.api.repository.ReceiptRepository;
import com.dwin.du.validation.ValidationException;
import com.dwin.du.validation.ValidationService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.ZoneId;
import java.util.*;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class ChartService {

    private final PersonRepository personRepository;
    private final ReceiptRepository receiptRepository;
    private final PersonProductRepository personProductRepository;
    private final ValidationService validator;

    private static final String[] MONTH_NAMES = {"Sty", "Lut", "Mar", "Kwi", "Maj", "Cze", "Lip", "Sie", "Wrz", "Paź", "Lis", "Gru"};
    private static final String[] WEEK_NAMES = {"Pon", "Wt", "Śr", "Czw", "Pt", "Sb", "Ndz"};

    public ResponseEntity<?> getChartAmounts(String username, boolean isTotalAmounts) {
        try {
            User user = validator.validateUser(username);
            List<Person> persons = personRepository.findByUser(user);

            List<ChartDto> responseList = new ArrayList<>();
            for (Person person : persons) {
                double amount = isTotalAmounts ? person.getTotalAmount() : person.getUnpaidAmount();
                if (Double.isNaN(amount))
                    amount = 0.0;
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

    public ResponseEntity<?> getMonthlyUserExpenses(String username, int year) {
        try {
            User user = validator.validateUser(username);

            List<Person> persons = personRepository.findByUser(user);
            List<Person> userAccountPersons = persons.stream()
                    .filter(Person::isUserAccount)
                    .toList();

            Map<Integer, Double> monthlyTotals = new HashMap<>();

            for (Person person : userAccountPersons) {
                List<PersonProduct> personProducts = personProductRepository.findByPerson(person);

                for (PersonProduct personProduct : personProducts) {
                    LocalDate date = personProduct.getProduct().getReceipt().getDate().toInstant().atZone(ZoneId.systemDefault()).toLocalDate();

                    if (date.getYear() == year) {
                        int month = date.getMonthValue();
                        monthlyTotals.put(month, monthlyTotals.getOrDefault(month, 0.0) + personProduct.getPartOfPrice());
                    }
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
        if (Double.isNaN(percentage))
            percentage = 0.0;

        return percentage;
        //return Math.round(percentage * 100.0) / 100.0;
    }


    public ResponseEntity<?> getWeeklyTotalExpenses(String username) {
        try {
            User user = validator.validateUser(username);
            List<Receipt> receipts = receiptRepository.findByUser(user);

            Map<DayOfWeek, Double> dailyTotals = new HashMap<>();
            for (DayOfWeek day : DayOfWeek.values())
                dailyTotals.put(day, 0.0);

            LocalDate today = LocalDate.now();
            LocalDate startOfWeek = today.with(DayOfWeek.MONDAY);
            LocalDate endOfWeek = today.with(DayOfWeek.SUNDAY);

            for (Receipt receipt : receipts) {
                LocalDate date = receipt.getDate().toInstant().atZone(ZoneId.systemDefault()).toLocalDate();

                if (!date.isBefore(startOfWeek) && !date.isAfter(endOfWeek)) {
                    DayOfWeek dayOfWeek = date.getDayOfWeek();
                    dailyTotals.put(dayOfWeek, dailyTotals.get(dayOfWeek) + receipt.getTotalPrice());
                }
            }

            List<ChartDto> responseList = new ArrayList<>();
            for (DayOfWeek day : DayOfWeek.values()) {
                double total = dailyTotals.getOrDefault(day, 0.0);
                if (Double.isNaN(total))
                    total = 0.0;

                responseList.add(ChartDto.builder()
                        .name(WEEK_NAMES[day.getValue() - 1])
                        .value(total)
                        .build());
            }


            return ResponseEntity.ok(responseList);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getWeeklyUserExpenses(String username) {
        try {
            User user = validator.validateUser(username);
            List<Person> persons = personRepository.findByUser(user);
            List<Person> userAccountPersons = persons.stream()
                    .filter(Person::isUserAccount)
                    .toList();

            Map<DayOfWeek, Double> dailyTotals = new HashMap<>();
            for (DayOfWeek day : DayOfWeek.values())
                dailyTotals.put(day, 0.0);

            LocalDate today = LocalDate.now();
            LocalDate startOfWeek = today.with(DayOfWeek.MONDAY);
            LocalDate endOfWeek = today.with(DayOfWeek.SUNDAY);

            for (Person person : userAccountPersons) {
                List<PersonProduct> personProducts = personProductRepository.findByPerson(person);

                for (PersonProduct personProduct : personProducts) {
                    LocalDate date = personProduct.getProduct().getReceipt().getDate().toInstant().atZone(ZoneId.systemDefault()).toLocalDate();

                    if (!date.isBefore(startOfWeek) && !date.isAfter(endOfWeek)) {
                        DayOfWeek dayOfWeek = date.getDayOfWeek();
                        dailyTotals.put(dayOfWeek, dailyTotals.get(dayOfWeek) + personProduct.getPartOfPrice());
                    }
                }
            }

            List<ChartDto> responseList = new ArrayList<>();
            for (DayOfWeek day : DayOfWeek.values()) {
                double total = dailyTotals.getOrDefault(day, 0.0);
                responseList.add(ChartDto.builder()
                        .name(WEEK_NAMES[day.getValue() - 1])
                        .value(total)
                        .build());
            }

            return ResponseEntity.ok(responseList);

        } catch (Exception e) {
            return handleException(e);
        }
    }

    public ResponseEntity<?> getMonthlyTopProducts(String username) {
        try {
            User user = validator.validateUser(username);
            List<Person> persons = personRepository.findByUser(user);

            List<PersonProduct> allPersonProducts = new ArrayList<>();
            for (Person person : persons) {
                allPersonProducts.addAll(personProductRepository.findByPerson(person));
            }

            LocalDate now = LocalDate.now();
            int currentMonth = now.getMonthValue();
            int currentYear = now.getYear();

            Map<String, Double> productValues = new HashMap<>();

            for (PersonProduct personProduct : allPersonProducts) {
                LocalDate productDate = personProduct.getProduct().getReceipt().getDate().toInstant().atZone(ZoneId.systemDefault()).toLocalDate();
                if (productDate.getMonthValue() == currentMonth && productDate.getYear() == currentYear) {
                    productValues.merge(personProduct.getProduct().getName(), personProduct.getPartOfPrice(), Double::sum);
                }
            }

            List<ChartDto> topProducts = productValues.entrySet().stream()
                    .sorted(Map.Entry.<String, Double>comparingByValue().reversed())
                    .limit(3)
                    .map(entry -> ChartDto.builder()
                            .name(entry.getKey())
                            .value(entry.getValue())
                            .build())
                    .collect(Collectors.toList());

            return ResponseEntity.ok(topProducts);

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
