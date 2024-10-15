package com.dwin.du.valid;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;

@ControllerAdvice
public class GlobalExceptionHandler {

    @ExceptionHandler(ValidException.class)
    public ResponseEntity<?> handleValidException(ValidException ex) {
        HttpStatus status = HttpStatus.resolve(ex.getErrorCode());
        if (status == null) {
            status = HttpStatus.BAD_REQUEST;
        }
        return ResponseEntity.status(status).body(ex.getMessage());
    }
}
