package com.dwin.rm.entity.person_product.Response;

import com.dwin.rm.entity.person.Person;
import com.dwin.rm.entity.product.Product;
import jakarta.persistence.*;
import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class ShowPersonProductRequest {
    private int personProductId;
    private int productId;
    private int personId;
    private double partOfPrice;
    private int quantity;
    private boolean isCompensation;
    private double compensationAmount;
    private  boolean isSettled;
}
