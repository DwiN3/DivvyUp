package com.dwin.du.entity.person_product.Request;

import lombok.Builder;
import lombok.Data;


@Builder
@Data
public class ChangePersonRequest {
    public int personId;
}