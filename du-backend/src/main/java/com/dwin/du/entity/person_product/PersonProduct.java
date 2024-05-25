package com.dwin.du.entity.person_product;
import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.product.Product;
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

    @Column(name = "max_quantity")
    private int maxQuantity;

    @Column(name = "is_compensation")
    private boolean isCompensation;

    @Column(name = "is_settled")
    private  boolean isSettled;
}