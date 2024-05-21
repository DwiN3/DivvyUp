package com.dwin.rm.entity.person_product.Request;

import lombok.Data;

@Data
public class AddPersonProductRequest {
    private int personId;
    private int quantity;
    private boolean isCompensation;
    private double compensationAmount;
}
