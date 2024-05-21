package com.dwin.rm.entity.product;
import com.dwin.rm.entity.receipt.Receipt;
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
    @Column(name = "product_id")
    private int productId;

    @ManyToOne
    @JoinColumn(name = "receipt_id")
    private Receipt receipt;

    @Column(name = "product_name", nullable = false)
    private String productName;

    @Column(name = "price", nullable = false)
    private Double price;

    @Column(name = "package_price")
    private double packagePrice;

    @Column(name = "parts_amount")
    private boolean divisible;

    @Column(name = "max_quantity")
    private int maxQuantity;

    @Column(name = "compensation_amount")
    private double compensationAmount;

    @Column(name = "isSettled")
    private  boolean isSettled;
}
