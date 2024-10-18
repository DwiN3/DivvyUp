package com.dwin.du.api.entity;
import jakarta.persistence.*;
import lombok.*;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "product")
public class Product {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private int id;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    @ManyToOne
    @JoinColumn(name = "receipt_id")
    private Receipt receipt;

    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "price", nullable = false)
    private Double price;

    @Column(name = "divisible")
    private boolean divisible;

    @Column(name = "max_quantity")
    private int maxQuantity;

    @Column(name = "compensation_price")
    private double compensationPrice;

    @Column(name = "is_settled")
    private boolean settled;
}
