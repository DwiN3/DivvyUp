package com.dwin.rm.entity.person;

import com.dwin.rm.entity.person.Request.AddPersonRequest;
import com.dwin.rm.entity.person.Request.SetPersonReceiptsCountsRequest;
import com.dwin.rm.entity.person.Request.SetTotalAmountReceiptRequest;
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
        return null;
    }

    @PutMapping("/edit/{personId}")
    public ResponseEntity<?> editPerson(@PathVariable int personId, @RequestBody AddPersonRequest request) {
        return null;
    }

    @DeleteMapping("/remove/{personId}")
    public ResponseEntity<?> removePerson(@PathVariable int personId) {
        return null;
    }

    @PutMapping("/set-receipts-counts/{personId}")
    public ResponseEntity<?> setReceiptsCounts(@PathVariable int personId, @RequestBody SetPersonReceiptsCountsRequest request) {
        return null;
    }

    @PutMapping("/set-total-purchase-amount/{personId}")
    public ResponseEntity<?> setTotalPurchaseAmount(@PathVariable int personId, @RequestBody SetTotalAmountReceiptRequest request) {
        return null;
    }

    @GetMapping("/show/{personId}")
    public ResponseEntity<?> showPerson(@PathVariable int personId) {
        return null;
    }

    @GetMapping("/show-all")
    public ResponseEntity<?> showPersons(){
        return null;
    }
}
