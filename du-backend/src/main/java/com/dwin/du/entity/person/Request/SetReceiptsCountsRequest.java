package com.dwin.du.entity.person.Request;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class SetReceiptsCountsRequest {
    public int receiptsCount;
}
