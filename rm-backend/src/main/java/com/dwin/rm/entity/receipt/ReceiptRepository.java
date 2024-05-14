package com.dwin.rm.entity.receipt;

import com.dwin.rm.entity.person.Person;
import com.dwin.rm.security.user.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ReceiptRepository extends JpaRepository<Receipt, Integer> {
    List<Receipt> findByUser(User user);
}
