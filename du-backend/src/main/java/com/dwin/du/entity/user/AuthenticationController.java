package com.dwin.du.entity.user;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import lombok.RequiredArgsConstructor;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm")
public class AuthenticationController {

    private final AuthenticationService service;

    @PostMapping("/register")
    public ResponseEntity<?> register(@RequestBody UserDto request){
        ResponseEntity<?> response = service.register(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PostMapping("/auth")
    public ResponseEntity<?> authenticate(@RequestBody UserDto request){
        ResponseEntity<String> response = service.auth(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @DeleteMapping("/remove-account")
    public ResponseEntity<?> removeAccount(@RequestBody UserDto request){
        ResponseEntity<?> response = service.remove(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/validate-token")
    public ResponseEntity<?> validateToken(@RequestParam String token) {
        ResponseEntity<?> response = service.validateToken(token);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }
}
