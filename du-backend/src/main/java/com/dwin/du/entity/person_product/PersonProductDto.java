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
    public int maxQuantity;
    public boolean isCompensation;
    public  boolean isSettled;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getProductId() {
        return productId;
    }

    public void setProductId(int productId) {
        this.productId = productId;
    }

    public int getPersonId() {
        return personId;
    }

    public void setPersonId(int personId) {
        this.personId = personId;
    }

    public double getPartOfPrice() {
        return partOfPrice;
    }

    public void setPartOfPrice(double partOfPrice) {
        this.partOfPrice = partOfPrice;
    }

    public int getQuantity() {
        return quantity;
    }

    public void setQuantity(int quantity) {
        this.quantity = quantity;
    }

    public int getMaxQuantity() {
        return maxQuantity;
    }

    public void setMaxQuantity(int maxQuantity) {
        this.maxQuantity = maxQuantity;
    }

    public boolean isCompensation() {
        return isCompensation;
    }

    public void setCompensation(boolean compensation) {
        isCompensation = compensation;
    }

    public boolean isSettled() {
        return isSettled;
    }

    public void setSettled(boolean settled) {
        isSettled = settled;
    }
}
