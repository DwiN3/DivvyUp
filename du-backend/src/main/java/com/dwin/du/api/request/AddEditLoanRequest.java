package com.dwin.du.api.request;
import lombok.Builder;
import lombok.Data;
import java.util.Date;

@Builder
@Data
public class AddEditLoanRequest {
    private int personId;
    private Date date;
    private Double amount;
    private boolean lent;
}
