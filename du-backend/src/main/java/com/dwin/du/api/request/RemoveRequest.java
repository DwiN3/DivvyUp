package com.dwin.du.api.request;
import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class RemoveRequest {
    public String password;
}
