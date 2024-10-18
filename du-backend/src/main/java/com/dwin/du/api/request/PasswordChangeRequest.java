package com.dwin.du.api.request;
import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class PasswordChangeRequest {
    public String password;
    public String newPassword;
}
