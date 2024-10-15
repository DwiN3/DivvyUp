package com.dwin.du.entity.product.Request;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class AddEditProductRequest {
    public String name;
    public Double price;
    public boolean divisible;
    public int maxQuantity;
}
