package com.dwin.du.entity.person;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class PersonDto {
    private int id;
    private String name;
    private String surname;
    private int receiptsCount;
    private Double totalAmount;
    private Double unpaidAmount;
}
