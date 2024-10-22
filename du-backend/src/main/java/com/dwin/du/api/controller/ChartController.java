package com.dwin.du.api.controller;
import com.dwin.du.api.service.ChartService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/chart")
@Tag(name = "Chart", description = "APIs for retrieving chart data.")
public class ChartController {

    private final ChartService personService;

    @GetMapping("/total-amounts")
    @Operation(summary = "Retrieve total amounts for chart", description = "Fetches chart data showing the total costs related to people associated with the user's account.")
    public ResponseEntity<?> getTotalAmounts() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.getChartAmounts(username, true);
    }

    @GetMapping("/unpaid-amounts")
    @Operation(summary = "Retrieve unpaid amounts for chart", description = "Fetches chart data showing the unpaid costs related to people associated with the user's account.")
    public ResponseEntity<?> getUnpaidAmounts() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.getChartAmounts(username, false);
    }

    @GetMapping("/percentage-expenses")
    @Operation(summary = "Retrieve percentage of expenses paid", description = "Fetches chart data showing the percentage of total costs paid by people associated with the user's account.")
    public ResponseEntity<?> getPercentageExpanses() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.getPercentageExpanses(username);
    }

    @GetMapping("/monthly-total-expenses/{year}")
    @Operation(summary = "Retrieve total expenses by month", description = "Fetches chart data showing the total expenses incurred by the user, grouped by each month. This data helps visualize monthly spending trends across all receipts and associated purchases")
    public ResponseEntity<?> getMonthlyTotalExpenses(@PathVariable int year) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return personService.getMonthlyTotalExpenses(username, year);
    }
}
