package com.dwin.du.valid;

public class ValidException extends RuntimeException {
    private final int errorCode;

    public ValidException(int errorCode) {
        this.errorCode = errorCode;
    }

    public int getErrorCode() {
        return errorCode;
    }
}

