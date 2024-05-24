package com.dwin.rm.security.user;

import com.dwin.rm.security.user.Request.AuthenticationRequest;
import com.dwin.rm.security.user.Request.RegisterRequest;
import com.dwin.rm.security.user.Request.RemoveAccountRequest;
import com.dwin.rm.security.user.Response.AuthenticationResponse;
import com.dwin.rm.security.config.JwtService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
@RequiredArgsConstructor
public class AuthenticationService {

    private final UserRepository repository;
    private final PasswordEncoder passwordEncoder;
    private final JwtService jwtService;
    private final AuthenticationManager authenticationManager;

    public ResponseEntity<AuthenticationResponse> register(RegisterRequest request) {
        if (repository.findByEmail(request.getEmail()).isPresent()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build(); 
        } else if (repository.findByUsername(request.getUsername()).isPresent()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }

        var user = User.builder()
                .username(request.getUsername())
                .email(request.getEmail())
                .password(passwordEncoder.encode(request.getPassword()))
                .role(Role.USER)
                .build();

        repository.save(user);

        var jwtToken = jwtService.generateTokem(user);
        return ResponseEntity.ok(AuthenticationResponse.builder().token(jwtToken).build());
    }

    public ResponseEntity<AuthenticationResponse> auth(AuthenticationRequest request) {
        try {
            Optional<User> optionalUser = repository.findByUsername(request.getUsername());
            if(!optionalUser.isPresent()){
                return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
            }

            User userExits = optionalUser.get();
            if (!passwordEncoder.matches(request.getPassword(), userExits.getPassword())) {
                return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
            }

            Authentication authentication = authenticationManager.authenticate(
                    new UsernamePasswordAuthenticationToken(
                            request.getUsername(),
                            request.getPassword()
                    )
            );

            SecurityContextHolder.getContext().setAuthentication(authentication);

            var user = repository.findByUsername(request.getUsername()).orElseThrow();

            var jwtToken = jwtService.generateTokem(user);
            return ResponseEntity.ok(AuthenticationResponse.builder().token(jwtToken).build());

        } catch (BadCredentialsException ex) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).build();
        }
    }

    public ResponseEntity<?> remove(RemoveAccountRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        Optional<User> optionalUser = repository.findByUsername(currentUsername);

        if (optionalUser.isPresent()) {
            User user = optionalUser.get();
            if (passwordEncoder.matches(request.getPassword(), user.getPassword())) {
                repository.delete(user);
                return ResponseEntity.ok().build();
            } else {
                return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
            }
        } else {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
    }
}
