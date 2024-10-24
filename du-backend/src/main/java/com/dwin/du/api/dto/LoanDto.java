package com.dwin.du.api.dto;
import lombok.Builder;
import lombok.Data;
import java.util.Date;

@Builder
@Data
public class LoanDto {
    public int id;
    public int personId;
    public Date date;
    public Double amount;
    public boolean lent;
    public boolean settled;
}
