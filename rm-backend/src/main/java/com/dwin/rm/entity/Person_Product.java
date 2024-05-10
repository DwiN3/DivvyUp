package com.dwin.rm.entity;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "person_product")
public class Person_Product {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "person_product_id")
    private int personProductId;

    @ManyToOne
    @JoinColumn(name = "product_id")
    private Product productId;

    @ManyToOne
    @JoinColumn(name = "person_id")
    private Person personId;

    @Column(name = "parts_count", nullable = false)
    private int partsCount;

    @Column(name = "is_compensation", nullable = false)
    private boolean isCompensation;

    @Column(name = "compensation_amount")
    private double compensationAmount;
}
