package com.dwin.rm.entity.person_product;

import com.dwin.rm.entity.person.Person;
import com.dwin.rm.entity.product.Product;
import com.dwin.rm.entity.receipt.Receipt;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface PersonProductRepository extends JpaRepository<PersonProduct, Integer> {
    List<PersonProduct> findByProduct(Product product);
    List<PersonProduct> findByPerson(Person person);
}
