package com.dwin.rm.entity.receipt.Response;

import lombok.Builder;
import lombok.Data;

import java.util.Date;

@Data
@Builder
public class ShowReceiptResponse {
    private int receiptId;
    private String receiptName;
    private Date date;
    private Double totalAmount;
    private boolean isSettled;
}
