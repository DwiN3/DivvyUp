package com.dwin.du.entity.person_product;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class PersonProductDto {
    public int id;
    public int productId;
    public int personId;
    public double partOfPrice;
    public int quantity;
    public boolean compensation;
    public  boolean settled;
}
