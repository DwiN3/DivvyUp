package com.dwin.rm.entity;
import com.dwin.rm.security.user.User;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.Date;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "receipt")
public class Receipt {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "receipt_id")
    private int receiptId;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User userId;

    @Column(name = "receipt_name", nullable = false)
    private String receiptName;

    @Column(name = "date")
    private Date date;

    @Column(name = "total_amount", nullable = false)
    private Double totalAmount;
}
