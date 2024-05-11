package com.dwin.rm.security.auth;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import lombok.RequiredArgsConstructor;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/")
public class AuthenticationController {

    private final AuthenticationService service;

    @PostMapping("/register")
    public ResponseEntity<?> register(@RequestBody RegisterRequest request){
        ResponseEntity<TokenResponse> response = service.register(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PostMapping("/auth")
    public ResponseEntity<?> authenticate(@RequestBody AuthenticationRequest request){
        ResponseEntity<TokenResponse> response = service.auth(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }
}
