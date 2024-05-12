package com.dwin.rm.entity.person_product.Request;

import jakarta.persistence.Column;
import lombok.Data;

@Data
public class AddPersonProductRequest {
    private int personId;
    private double partOfPrice;
    private int quantity;
    private boolean isCompensation;
    private double compensationAmount;
}
