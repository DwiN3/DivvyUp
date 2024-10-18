package com.dwin.du.entity.receipt.Request;
import lombok.Builder;
import lombok.Data;
import java.util.Date;

@Builder
@Data
public class AddEditReceiptRequest {
    public String name;
    public Date date;
}
