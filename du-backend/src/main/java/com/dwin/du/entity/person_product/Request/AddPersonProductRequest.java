package com.dwin.du.entity.person_product.Request;

import lombok.Data;

@Data
public class AddPersonProductRequest {
    private int personId;
    private int quantity;
}
