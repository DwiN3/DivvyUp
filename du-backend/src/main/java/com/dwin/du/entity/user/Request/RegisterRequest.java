package com.dwin.du.entity.user.Request;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class RegisterRequest {
    public String username;
    public String email;
    public String password;
}
