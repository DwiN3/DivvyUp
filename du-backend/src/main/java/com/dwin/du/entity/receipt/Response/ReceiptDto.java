package com.dwin.du.entity.receipt.Response;

import lombok.Builder;
import lombok.Data;

import java.util.Date;

@Data
@Builder
public class ReceiptDto {
    private int id;
    private String receiptName;
    private Date date;
    private Double totalAmount;
    private boolean isSettled;
}
