package com.dwin.rm.entity.person;

import com.dwin.rm.entity.person.Request.AddPersonRequest;
import com.dwin.rm.entity.person.Request.SetPersonReceiptsCountsRequest;
import com.dwin.rm.entity.person.Request.SetPersonTotalPurchaseAmountRequest;
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
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personService.addPerson(request, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PutMapping("/edit/{personId}")
    public ResponseEntity<?> editPerson(@PathVariable int personId, @RequestBody AddPersonRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personService.editPerson(personId, request, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @DeleteMapping("/remove/{personId}")
    public ResponseEntity<?> removePerson(@PathVariable int personId){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personService.removePerson(personId, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/show/{personId}")
    public ResponseEntity<?> showPersonById(@PathVariable int personId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personService.showPersonById(personId, currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/show-all")
    public ResponseEntity<?> showPersons(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personService.showPersons(currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PutMapping("/set-receipts-counts/{personId}")
    public ResponseEntity<?> setReceiptsCounts(@PathVariable int personId, @RequestBody SetPersonReceiptsCountsRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personService.setReceiptsCounts(personId, request.getReceiptsCount(), currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PutMapping("/set-total-purchase-amount/{personId}")
    public ResponseEntity<?> setTotalPurchaseAmount(@PathVariable int personId, @RequestBody SetPersonTotalPurchaseAmountRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        ResponseEntity<?> response = personService.setTotalPurchaseAmount(personId, request.getTotalPurchaseAmount(), currentUsername);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }
}
