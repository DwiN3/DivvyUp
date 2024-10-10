package com.dwin.du.entity.user;

import com.dwin.du.config.JwtService;
import com.dwin.du.entity.user.Request.LoginRequest;
import com.dwin.du.entity.user.Request.RegisterRequest;
import com.dwin.du.entity.user.Request.RemoveRequest;
import com.dwin.du.valid.ValidException;
import com.dwin.du.valid.ValidService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
@RequiredArgsConstructor
public class UserService {

    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final JwtService jwtService;
    private final AuthenticationManager authenticationManager;
    private final ValidService valid;

    public ResponseEntity<?> register(RegisterRequest request) {
        if (userRepository.findByEmail(request.getEmail()).isPresent()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        } else if (userRepository.findByUsername(request.getUsername()).isPresent()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }

        var user = User.builder()
                .username(request.getUsername())
                .email(request.getEmail())
                .password(passwordEncoder.encode(request.getPassword()))
                .role(Role.USER)
                .build();

        userRepository.save(user);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<String> auth(LoginRequest request) {
        try {
            Optional<User> optionalUser = userRepository.findByUsername(request.getUsername());
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

            var user = userRepository.findByUsername(request.getUsername()).orElseThrow();
            var jwtToken = jwtService.generateTokem(user);
            return ResponseEntity.ok(jwtToken);

        } catch (BadCredentialsException ex) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).build();
        }
    }

    public ResponseEntity<?> remove(String username) {
        User user = valid.validateUser(username);
        userRepository.delete(user);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> validateToken(String token) {
        try {
            String username = jwtService.extractUsername(token);
            UserDetails userDetails = userRepository.findByUsername(username).orElseThrow(() -> new UsernameNotFoundException("User not found"));
            boolean isValid = jwtService.isTokenValid(token, userDetails);

            if (isValid) {
                return ResponseEntity.ok(true);
            } else {
                return ResponseEntity.ok(false);
            }
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    public ResponseEntity<?> editUser(String username, UserDto request) {
        User user = valid.validateUser(username);

        user.setUsername(request.getUsername());
        user.setEmail(request.getEmail());
        userRepository.save(user);

        String newToken = jwtService.generateTokem(user);
        
        return ResponseEntity.ok(newToken);
    }

    public ResponseEntity<?> getUser(String token) {
        try {
            String username = jwtService.extractUsername(token);
            User user = userRepository.findByUsername(username).orElseThrow(() -> new ValidException(404));
            boolean isValid = jwtService.isTokenValid(token, user);
            if(isValid){
                UserDto response = UserDto.builder()
                        .id(user.getId())
                        .username(user.getUsername())
                        .email(user.getEmail())
                        .password(user.getPassword())
                        .build();
                return ResponseEntity.ok(response);
            } else{
                throw new ValidException(401);
            }
        } catch (Exception e) {
            throw new ValidException(401);
        }
    }
}
