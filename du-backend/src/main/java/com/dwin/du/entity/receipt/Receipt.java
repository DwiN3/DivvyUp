package com.dwin.du.entity.receipt;
import com.dwin.du.security.user.User;
import com.fasterxml.jackson.annotation.JsonProperty;
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
    @JsonProperty("ReceiptId")
    @Column(name = "receipt_id")
    private int receiptId;

    @ManyToOne
    @JsonProperty("UserId")
    @JoinColumn(name = "user_id")
    private User user;

    @Column(name = "receipt_name", nullable = false)
    @JsonProperty("ReceiptName")
    private String receiptName;

    @Column(name = "date")
    private Date date;

    @Column(name = "total_amount")
    @JsonProperty("TotalAmount")
    private Double totalAmount;

    @Column(name = "is_settled")
    @JsonProperty("IsSettled")
    private boolean isSettled;
}
