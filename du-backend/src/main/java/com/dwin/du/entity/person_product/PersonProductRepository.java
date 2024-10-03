package com.dwin.du.entity.person_product;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.product.Product;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;
import java.util.Optional;

@Repository
public interface PersonProductRepository extends JpaRepository<PersonProduct, Integer> {
    List<PersonProduct> findByProduct(Product product);
    List<PersonProduct> findByPerson(Person person);
    Optional<PersonProduct> findByProductAndPerson(Product product, Person person);
}
