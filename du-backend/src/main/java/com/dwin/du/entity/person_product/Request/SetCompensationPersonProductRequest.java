package com.dwin.du.entity.person_product.Request;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class SetCompensationPersonProductRequest {
    public boolean isCompensation;
}
