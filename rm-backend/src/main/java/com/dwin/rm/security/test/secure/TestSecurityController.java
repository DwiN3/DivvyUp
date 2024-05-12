package com.dwin.rm.security.test.secure;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/rm/secure")
public class TestSecurityController {

    @GetMapping
    public ResponseEntity<String> showMessage(){
        return ResponseEntity.ok("Its secure message");
    }
}
