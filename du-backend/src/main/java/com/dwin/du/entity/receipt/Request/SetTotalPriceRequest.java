package com.dwin.du.entity.receipt.Request;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class SetTotalPriceRequest {
    public Double totalPrice;
}
