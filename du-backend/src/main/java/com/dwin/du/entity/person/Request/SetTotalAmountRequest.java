package com.dwin.du.entity.person.Request;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class SetTotalAmountRequest {
    public Double totalAmount;
}
