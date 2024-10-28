package com.dwin.du.api.repository;
import com.dwin.du.api.entity.*;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface ProductRepository extends JpaRepository<Product, Integer> {
    List<Product> findByReceipt(Receipt receipt);
    List<Product> findByReceiptIn(List<Receipt> receipts);
}
