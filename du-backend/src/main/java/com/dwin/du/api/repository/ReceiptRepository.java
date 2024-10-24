package com.dwin.du.api.repository;
import com.dwin.du.api.entity.Receipt;
import com.dwin.du.api.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Date;
import java.util.List;

@Repository
public interface ReceiptRepository extends JpaRepository<Receipt, Integer> {
    List<Receipt> findByUser(User user);
    List<Receipt> findByUserAndDateBetween(User user, Date dateFrom, Date dateTo);
}
