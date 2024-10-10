package com.dwin.du.entity.user.Request;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class PasswordChangeRequest {
    public String password;
    public String newPassword;
}
