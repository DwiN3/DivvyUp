package com.dwin.du.entity.person_product.Response;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class ShowPersonProductResponse {
    private int personProductId;
    private int productId;
    private int personId;
    private double partOfPrice;
    private int quantity;
    private int maxQuantity;
    private boolean isCompensation;
    private  boolean isSettled;
}
