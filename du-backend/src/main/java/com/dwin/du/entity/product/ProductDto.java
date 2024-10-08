package com.dwin.du.entity.product;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class ProductDto {
    public int id;
    public int receiptId;
    public String name;
    public Double price;
    public Double compensationPrice;
    public boolean divisible;
    public int maxQuantity;
    public  boolean settled;
}
