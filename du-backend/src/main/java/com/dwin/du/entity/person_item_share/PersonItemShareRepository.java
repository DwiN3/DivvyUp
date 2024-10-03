package com.dwin.du.entity.person_item_share;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.item.Item;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;
import java.util.Optional;

@Repository
public interface PersonItemShareRepository extends JpaRepository<PersonItemShare, Integer> {
    List<PersonItemShare> findByItem(Item item);
    List<PersonItemShare> findByPerson(Person person);
    Optional<PersonItemShare> findByItemAndPerson(Item item, Person person);
}
