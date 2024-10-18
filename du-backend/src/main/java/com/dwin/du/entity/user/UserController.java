package com.dwin.du.entity.user;

import com.dwin.du.entity.user.Request.EditUserRequest;
import com.dwin.du.entity.user.Request.LoginRequest;
import com.dwin.du.entity.user.Request.PasswordChangeRequest;
import com.dwin.du.entity.user.Request.RegisterRequest;
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
@Tag(name = "User Management", description = "APIs for user authentication and account management")
public class UserController {

    private final UserService userService;

    @PostMapping("/register")
    @Operation(summary = "Register a new user", description = "Registers a new user account in the system.")
    public ResponseEntity<?> register(@RequestBody RegisterRequest request){
        ResponseEntity<?> response = userService.register(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PostMapping("/auth")
    @Operation(summary = "Authenticate user", description = "Authenticates a user and returns an authentication token.")
    public ResponseEntity<?> authenticate(@RequestBody LoginRequest request){
        ResponseEntity<String> response = userService.authenticate(request);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/validate-token")
    @Operation(summary = "Validate token", description = "Checks the validity of a given authentication token")
    public ResponseEntity<?> validateToken(@RequestParam String token) {
        ResponseEntity<?> response = userService.validateToken(token);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @PutMapping("/user/change-password")
    @Operation(summary = "Change user password", description = "Changes the password for the currently authenticated user.")
    public ResponseEntity<?> changePassword(@RequestBody PasswordChangeRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return userService.changePassword(username, request);
    }

    @PutMapping("/user/edit")
    @Operation(summary = "Edit user account", description = "Edits the details of the currently authenticated user.")
    public ResponseEntity<?> editUser(@RequestBody EditUserRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return userService.editUser(username, request);
    }

    @DeleteMapping("/user/remove")
    @Operation(summary = "Remove user account", description = "Removes the currently authenticated user's account.")
    public ResponseEntity<?> removeUser(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String username = authentication.getName();
        return userService.removeUser(username);
    }

    @GetMapping("/user")
    @Operation(summary = "Retrieve user by token", description = "Retrieves the user information associated with the provided token.")
    public ResponseEntity<?> getUser(@RequestParam String token){
        return userService.getUser(token);
    }
}
