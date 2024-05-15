package com.dwin.rm.entity.receipt;
import com.dwin.rm.security.user.User;
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
    @Column(name = "receipt_id")
    private int receiptId;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    @Column(name = "receipt_name", nullable = false)
    private String receiptName;

    @Column(name = "date")
    private Date date;

    @Column(name = "total_amount")
    private Double totalAmount;

    @Column(name = "is_settled")
    private boolean isSettled;
}
