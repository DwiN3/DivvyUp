package com.dwin.rm.entity.receipt;
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

    @Column(name = "added_by_user_id")
    private int addedByUserId;

    @Column(name = "receipt_name", nullable = false)
    private String receiptName;

    @Column(name = "date")
    private Date date;

    @Column(name = "total_amount")
    private Double totalAmount;
}
