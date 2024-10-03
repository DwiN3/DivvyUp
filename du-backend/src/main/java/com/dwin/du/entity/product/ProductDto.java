package com.dwin.du.entity.product;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class ProductDto {
    private int id;
    private int receiptId;
    private String productName;
    private Double price;
    private Double compensationAmount;
    private double packagePrice;
    private boolean divisible;
    private int maxQuantity;
    private  boolean isSettled;
}
