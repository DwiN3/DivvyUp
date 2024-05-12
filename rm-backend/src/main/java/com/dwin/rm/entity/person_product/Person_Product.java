package com.dwin.rm.entity.person_product;
import jakarta.persistence.*;
import lombok.*;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "person_product")
public class Person_Product {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "person_product_id")
    private int personProductId;

    @Column(name = "product_id")
    private int productId;

    @Column(name = "person_id")
    private int personId;

    @Column(name = "quantity", nullable = false)
    private int quantity;

    @Column(name = "is_compensation", nullable = false)
    private boolean isCompensation;

    @Column(name = "compensation_amount")
    private double compensationAmount;
}
