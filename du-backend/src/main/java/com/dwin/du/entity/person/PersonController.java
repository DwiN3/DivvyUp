package com.dwin.du.entity.person;

import com.dwin.du.entity.person.Request.AddPersonRequest;
import com.dwin.du.entity.person.Request.SetPersonReceiptsCountsRequest;
import com.dwin.du.entity.person.Request.SetTotalAmountReceiptRequest;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;
import lombok.RequiredArgsConstructor;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/person")
public class PersonController {

  private final PersonService personService;

    @PostMapping("/add")
    public ResponseEntity<?> addPerson(@RequestBody AddPersonRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if (authentication == null || !authentication.isAuthenticated())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        String currentUsername = authentication.getName();
        return personService.addPerson(request, currentUsername);
    }

    @PutMapping("/edit/{personId}")
    public ResponseEntity<?> editPerson(@PathVariable int personId, @RequestBody AddPersonRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.editPerson(personId, request, currentUsername);
    }

    @DeleteMapping("/remove/{personId}")
    public ResponseEntity<?> removePerson(@PathVariable int personId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.removePerson(personId, currentUsername);
    }

    @PutMapping("/set-receipts-counts/{personId}")
    public ResponseEntity<?> setReceiptsCounts(@PathVariable int personId, @RequestBody SetPersonReceiptsCountsRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.setReceiptsCounts(personId, request, currentUsername);
    }

    @PutMapping("/set-total-purchase-amount/{personId}")
    public ResponseEntity<?> setTotalPurchaseAmount(@PathVariable int personId, @RequestBody SetTotalAmountReceiptRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.setTotalPurchaseAmount(personId, request, currentUsername);
    }

    @GetMapping("/show/{personId}")
    public ResponseEntity<?> showPerson(@PathVariable int personId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.showPerson(personId, currentUsername);
    }

    @GetMapping("/show-all")
    public ResponseEntity<?> showPersons(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personService.showPersons(currentUsername);
    }
}
