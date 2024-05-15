package com.dwin.rm.entity.product.Response;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class ShowProductResponse {
    private int productId;
    private int receiptId;
    private String productName;
    private Double price;
    private double packagePrice;
    private boolean divisible;
    private int maxQuantity;
    private  boolean isSettled;
}
