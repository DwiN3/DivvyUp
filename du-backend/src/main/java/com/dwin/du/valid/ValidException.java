package com.dwin.du.valid;

public class ValidException extends RuntimeException {
    private final int errorCode;
    private final String message;

    public ValidException(int errorCode, String message) {
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

