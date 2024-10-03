package com.dwin.du.entity.person_item_share;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class PersonItemShareDto {
    public int id;
    public int itemId;
    public int personId;
    public double partOfPrice;
    public int quantity;
    public int maxQuantity;
    public boolean isCompensation;
    public  boolean isSettled;
}
