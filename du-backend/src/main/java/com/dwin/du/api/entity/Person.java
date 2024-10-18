package com.dwin.du.api.entity;
import jakarta.persistence.*;
import lombok.*;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "person")
public class Person {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private int id;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "surname")
    private String surname;

    @Column(name = "receipts_count")
    private int receiptsCount;

    @Column(name = "product_count")
    private int productsCount;

    @Column(name = "total_amount")
    private Double totalAmount;

    @Column(name = "unpaid_amount")
    private Double unpaidAmount;

    @Column(name = "is_user_account")
    private boolean userAccount;
}
