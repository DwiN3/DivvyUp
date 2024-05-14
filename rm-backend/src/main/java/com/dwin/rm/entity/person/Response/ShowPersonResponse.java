package com.dwin.rm.entity.person.Response;

import com.dwin.rm.security.user.User;
import jakarta.persistence.*;
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
