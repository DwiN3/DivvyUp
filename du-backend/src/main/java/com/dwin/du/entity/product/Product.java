package com.dwin.du.entity.product;
import com.dwin.du.entity.receipt.Receipt;
import jakarta.persistence.*;
import lombok.*;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "product")
public class Product {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private int id;

    @ManyToOne
    @JoinColumn(name = "receipt_id")
    private Receipt receipt;

    @Column(name = "product_name", nullable = false)
    private String productName;

    @Column(name = "price", nullable = false)
    private Double price;

    @Column(name = "package_price")
    private double packagePrice;

    @Column(name = "divisible")
    private boolean divisible;

    @Column(name = "max_quantity")
    private int maxQuantity;

    @Column(name = "compensation_amount")
    private double compensationAmount;

    @Column(name = "is_settled")
    private boolean isSettled;
}
