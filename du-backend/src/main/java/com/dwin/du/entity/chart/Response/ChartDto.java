package com.dwin.du.entity.chart.Response;

import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class ChartDto {
    public String Name;
    public double Value;
}
