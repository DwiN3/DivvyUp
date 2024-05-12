package com.dwin.rm.entity.person.Response;

import lombok.Data;

@Data
public class AddPersonResponse {
    private String name;
    private String surname;
    private int receiptsCount;
    private double totalPurchaseAmount;
}
