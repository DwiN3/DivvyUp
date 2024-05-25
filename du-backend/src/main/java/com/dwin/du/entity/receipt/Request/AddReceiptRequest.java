package com.dwin.du.entity.receipt.Request;

import lombok.Data;

import java.util.Date;

@Data
public class AddReceiptRequest {
    private String receiptName;
    private Date date;
}
