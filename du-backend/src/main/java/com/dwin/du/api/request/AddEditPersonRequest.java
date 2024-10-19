package com.dwin.du.api.request;
import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class AddEditPersonRequest {
    public String name;
    public String surname;

    public AddEditPersonRequest(String name, String surname) {
        this.name = name;
        this.surname = surname;
    }
}
