package com.dwin.rm.entity.receipt.Request;

import com.dwin.rm.security.user.User;
import jakarta.persistence.*;
import lombok.Builder;
import lombok.Data;

import java.util.Date;

@Data
@Builder
public class ShowReceiptRequest {
    private int receiptId;
    private String receiptName;
    private Date date;
    private Double totalAmount;
    private  boolean isSettled;
}
