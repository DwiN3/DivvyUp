package com.dwin.rm.entity.product.Request;

import jakarta.persistence.Column;
import lombok.Data;

@Data
public class AddProductRequest {
    private String productName;
    private Double price;
    private double packagePrice;
    private boolean divisible;
}