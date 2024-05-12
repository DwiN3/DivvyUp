package com.dwin.rm.entity.person_product;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface PersonProductRepository extends JpaRepository<PersonProduct, Integer> {
    List<PersonProduct> findAllByProductId(int productId);
    List<PersonProduct> findAllByProductIdIn(List<Integer> productIds);
}
