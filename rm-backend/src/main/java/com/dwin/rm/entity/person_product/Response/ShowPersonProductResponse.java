package com.dwin.rm.entity.person_product.Response;

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
    private boolean isCompensation;
    private double compensationAmount;
    private  boolean isSettled;
}
