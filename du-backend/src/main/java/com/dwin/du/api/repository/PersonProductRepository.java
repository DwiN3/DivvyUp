package com.dwin.du.api.repository;
import com.dwin.du.api.entity.Person;
import com.dwin.du.api.entity.PersonProduct;
import com.dwin.du.api.entity.Product;
import com.dwin.du.api.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;
import java.util.Optional;

@Repository
public interface PersonProductRepository extends JpaRepository<PersonProduct, Integer> {
    List<PersonProduct> findByUser(User user);
    List<PersonProduct> findByProduct(Product product);
    List<PersonProduct> findByPerson(Person person);
    Optional<PersonProduct> findByProductAndPerson(Product product, Person person);
}
