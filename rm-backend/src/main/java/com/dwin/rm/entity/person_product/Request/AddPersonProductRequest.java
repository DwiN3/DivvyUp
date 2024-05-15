package com.dwin.rm.entity.person_product.Request;

import com.dwin.rm.entity.person.Person;
import jakarta.persistence.Column;
import lombok.Data;

@Data
public class AddPersonProductRequest {
    private Person person;
    private double partOfPrice;
    private int quantity;
    private int maxQuantity;
    private boolean isCompensation;
    private double compensationAmount;
}
