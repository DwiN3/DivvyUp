package com.dwin.du.entity.user.Request;
import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class LoginRequest {
    public String username;
    public String password;
}
