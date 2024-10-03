package com.dwin.du.entity.item;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class ItemDto {
    public int id;
    public int receiptId;
    public String name;
    public Double price;
    public Double compensationPrice;
    public boolean divisible;
    public int maxQuantity;
    public  boolean isSettled;
}
