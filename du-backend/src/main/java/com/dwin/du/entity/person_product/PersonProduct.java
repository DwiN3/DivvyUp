package com.dwin.du.entity.person_product;
import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.user.User;
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
    @JoinColumn(name = "user_id")
    private User user;

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