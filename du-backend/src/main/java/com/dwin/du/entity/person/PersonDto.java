package com.dwin.du.entity.person;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class PersonDto {
    public int id;
    public String name;
    public String surname;
    public int receiptsCount;
    public Double totalAmount;
    public Double unpaidAmount;
}
