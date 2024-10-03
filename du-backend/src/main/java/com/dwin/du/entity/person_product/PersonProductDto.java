package com.dwin.du.entity.person_product;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class PersonProductDto {
    private int id;
    private int productId;
    private int personId;
    private double partOfPrice;
    private int quantity;
    private int maxQuantity;
    private boolean isCompensation;
    private  boolean isSettled;
}
