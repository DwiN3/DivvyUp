package com.dwin.du.api.repository;
import com.dwin.du.api.entity.Product;
import com.dwin.du.api.entity.Receipt;
import com.dwin.du.api.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface ProductRepository extends JpaRepository<Product, Integer> {
    List<Product> findByReceipt(Receipt receipt);
    List<Product> findByUser(User user);
}
