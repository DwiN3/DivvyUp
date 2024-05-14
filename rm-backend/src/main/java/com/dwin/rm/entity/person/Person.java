package com.dwin.rm.entity.person;
import com.dwin.rm.security.user.User;
import com.dwin.rm.security.user.UserRepository;
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
    @Column(name = "person_id")
    private int personId;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "surname")
    private String surname;

    @Column(name = "receipts_count")
    private int receiptsCount;

    @Column(name = "total_purchase_amount")
    private Double totalPurchaseAmount;
}
