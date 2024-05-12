package com.dwin.rm.entity.person_product;
import jakarta.persistence.*;
import lombok.*;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "person_product")
public class PersonProduct {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "person_product_id")
    private int personProductId;

    @Column(name = "added_by_user_id", nullable = false)
    private int addedByUserId;

    @Column(name = "product_id", nullable = false)
    private int productId;

    @Column(name = "person_id", nullable = false)
    private int personId;

    @Column(name = "part_of_price")
    private double partOfPrice;

    @Column(name = "quantity")
    private int quantity;

    @Column(name = "is_compensation")
    private boolean isCompensation;

    @Column(name = "compensation_amount")
    private double compensationAmount;
}
