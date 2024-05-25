package com.dwin.du.security.user;

import com.dwin.du.security.user.Request.RegisterRequest;
import com.dwin.du.security.user.Request.AuthenticationRequest;
import com.dwin.du.security.user.Request.RemoveAccountRequest;
import com.dwin.du.security.user.Response.AuthenticationResponse;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import lombok.RequiredArgsConstructor;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm/")
public class AuthenticationController {

    private final AuthenticationService service;

    @PostMapping("/register")
    public ResponseEntity<?> register(@RequestBody RegisterRequest request){
        ResponseEntity<AuthenticationResponse> response = service.register(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PostMapping("/auth")
    public ResponseEntity<?> authenticate(@RequestBody AuthenticationRequest request){
        ResponseEntity<AuthenticationResponse> response = service.auth(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @DeleteMapping("/remove-account")
    public ResponseEntity<?> removeAccount(@RequestBody RemoveAccountRequest request){
        ResponseEntity<?> response = service.remove(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }
}
