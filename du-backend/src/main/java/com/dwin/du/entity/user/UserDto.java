package com.dwin.du.entity.user;
import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class UserDto {
    public  int id;
    public String username;
    public String email;
    public String password;
}
