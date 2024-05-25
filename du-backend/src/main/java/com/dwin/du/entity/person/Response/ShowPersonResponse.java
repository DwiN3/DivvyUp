package com.dwin.du.entity.person.Response;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class ShowPersonResponse {
    private int personId;
    private String name;
    private String surname;
    private int receiptsCount;
    private Double totalPurchaseAmount;
}
