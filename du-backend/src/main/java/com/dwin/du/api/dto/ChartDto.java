package com.dwin.du.api.dto;
import lombok.Builder;
import lombok.Data;

@Builder
@Data
public class ChartDto {
    public String name;
    public double value;
}
