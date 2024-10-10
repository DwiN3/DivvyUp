package com.dwin.du.entity.product;

import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.user.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ProductRepository extends JpaRepository<Product, Integer> {
    List<Product> findByReceipt(Receipt receipt);
    List<Product> findByUser(User user);
}
