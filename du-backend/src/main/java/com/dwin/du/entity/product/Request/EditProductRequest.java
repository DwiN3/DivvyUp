package com.dwin.du.entity.product.Request;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class EditProductRequest {
    public String name;
}
