package com.dwin.du.validation;

public class ValidationException extends RuntimeException {
    private final int errorCode;
    private final String message;

    public ValidationException(int errorCode, String message) {
        super(message);
        this.errorCode = errorCode;
        this.message = message;
    }

    public int getErrorCode() {
        return errorCode;
    }

    public String getMessage() {
        return message;
    }
}

