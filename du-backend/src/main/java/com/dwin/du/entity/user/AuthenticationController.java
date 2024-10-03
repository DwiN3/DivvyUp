package com.dwin.du.entity.user;

import com.dwin.du.entity.user.Request.LoginRequest;
import com.dwin.du.entity.user.Request.RegisterRequest;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import lombok.RequiredArgsConstructor;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm")
@Tag(name = "Authentication Management", description = "APIs for user authentication and account management")
public class AuthenticationController {

    private final AuthenticationService service;

    @PostMapping("/register")
    @Operation(summary = "Register a new user", description = "Registers a new user account in the system.")
    public ResponseEntity<?> register(@RequestBody RegisterRequest request){
        ResponseEntity<?> response = service.register(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PostMapping("/auth")
    @Operation(summary = "Authenticate user", description = "Authenticates a user and provides a token.")
    public ResponseEntity<?> authenticate(@RequestBody LoginRequest request){
        ResponseEntity<String> response = service.auth(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @DeleteMapping("/remove-account")
    @Operation(summary = "Remove user account", description = "Removes a user account from the system.")
    public ResponseEntity<?> removeAccount(@RequestBody UserDto request){
        ResponseEntity<?> response = service.remove(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/validate-token")
    @Operation(summary = "Validate token", description = "Checks the validity of the token")
    public ResponseEntity<?> validateToken(@RequestParam String token) {
        ResponseEntity<?> response = service.validateToken(token);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }
}
