package com.dwin.du.api.entity;
import jakarta.persistence.*;
import lombok.*;
import java.util.Date;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "receipt")
public class Receipt {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private int id;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "date")
    private Date date;

    @Column(name = "total_price")
    private Double totalPrice;

    @Column(name = "is_settled")
    private boolean settled;
}
