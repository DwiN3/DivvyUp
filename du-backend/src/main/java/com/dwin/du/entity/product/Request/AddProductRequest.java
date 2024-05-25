package com.dwin.du.entity.product.Request;

import lombok.Data;

@Data
public class AddProductRequest {
    private String productName;
    private Double price;
    private double packagePrice;
    private boolean divisible;
    private int maxQuantity;
}