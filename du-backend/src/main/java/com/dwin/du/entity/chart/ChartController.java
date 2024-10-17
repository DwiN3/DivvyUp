package com.dwin.du.entity.chart;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/chart")
@Tag(name = "Chart", description = "APIs for getting charts")
public class ChartController {

    private final ChartService personService;

    @GetMapping("/total-amounts")
    @Operation(summary = "Get persons total amount to chart", description = "Retrieves all persons data to chart.")
    public ResponseEntity<?> getTotalAmounts() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getChartAmounts(currentUsername, true);
    }

    @GetMapping("/unpaid-amounts")
    @Operation(summary = "Get persons unpaid amount to chart", description = "Retrieves all persons data to chart.")
    public ResponseEntity<?> getUnpaidAmounts() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getChartAmounts(currentUsername, false);
    }

    @GetMapping("/percentage-expenses")
    @Operation(summary = "Get persons percentage expenses to chart", description = "Retrieves all persons data to chart.")
    public ResponseEntity<?> getPercentageExpanses() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.getPercentageExpanses(currentUsername);
    }
}
