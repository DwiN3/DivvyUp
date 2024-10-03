package com.dwin.du.entity.person;
import com.dwin.du.entity.user.User;
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

    @Column(name = "total_amount")
    private Double totalAmount;

    @Column(name = "unpaid_amount")
    private Double unpaidAmount;
}
