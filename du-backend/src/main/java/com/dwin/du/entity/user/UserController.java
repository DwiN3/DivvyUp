package com.dwin.du.entity.user;

import com.dwin.du.entity.user.Request.LoginRequest;
import com.dwin.du.entity.user.Request.RegisterRequest;
import com.dwin.du.entity.user.Request.RemoveRequest;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;
import lombok.RequiredArgsConstructor;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm")
@Tag(name = "Authentication Management", description = "APIs for user authentication and account management")
public class UserController {

    private final UserService userService;

    @PostMapping("/register")
    @Operation(summary = "Register a new user", description = "Registers a new user account in the system.")
    public ResponseEntity<?> register(@RequestBody RegisterRequest request){
        ResponseEntity<?> response = userService.register(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PostMapping("/auth")
    @Operation(summary = "Authenticate user", description = "Authenticates a user and provides a token.")
    public ResponseEntity<?> authenticate(@RequestBody LoginRequest request){
        ResponseEntity<String> response = userService.auth(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/validate-token")
    @Operation(summary = "Validate token", description = "Checks the validity of the token")
    public ResponseEntity<?> validateToken(@RequestParam String token) {
        ResponseEntity<?> response = userService.validateToken(token);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PutMapping("/user/edit")
    @Operation(summary = "Edit a user", description = "Edits an existing user.")
    public ResponseEntity<?> editUser(@RequestBody UserDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return userService.editUser(currentUsername, request);
    }

    @PutMapping("/user/remove")
    @Operation(summary = "Remove user account", description = "Removes a user account from the system.")
    public ResponseEntity<?> removeUser(@RequestBody RemoveRequest request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return userService.remove(currentUsername);
    }

    @GetMapping("/user")
    @Operation(summary = "Return user", description = "Get user using token.")
    public ResponseEntity<?> getUser(@RequestParam String token){
        return userService.getUser(token);
    }
}
