package com.dwin.rm.entity.person.Request;

import lombok.Data;

@Data
public class AddPersonRequest {
    private String name;
    private String surname;
    private int receiptsCount;
    private double totalPurchaseAmount;
}