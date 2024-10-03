package com.dwin.du.entity.receipt;

import lombok.Builder;
import lombok.Data;

import java.util.Date;

@Data
@Builder
public class ReceiptDto {
    public int id;
    public String name;
    public Date date;
    public Double totalPrice;
    public boolean isSettled;
}
