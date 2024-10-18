package com.dwin.du.api.request;
import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class LoginRequest {
    public String username;
    public String password;
}
