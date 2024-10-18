package com.dwin.du.api.request;
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
