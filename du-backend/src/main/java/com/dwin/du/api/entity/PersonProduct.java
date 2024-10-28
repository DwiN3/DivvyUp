package com.dwin.du.api.entity;
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
    @Column(name = "id")
    private int id;

    @ManyToOne
    @JoinColumn(name = "product_id")
    private Product product;

    @ManyToOne
    @JoinColumn(name = "person_id")
    private Person person;

    @Column(name= "part_of_price")
    private double partOfPrice;

    @Column(name = "quantity")
    private int quantity;

    @Column(name = "is_compensation")
    private boolean compensation;

    @Column(name = "is_settled")
    private  boolean settled;
}