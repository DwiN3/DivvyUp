package com.dwin.du.entity.item;

import com.dwin.du.entity.receipt.Receipt;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ItemRepository extends JpaRepository<Item, Integer> {
    List<Item> findByReceipt(Receipt receipt);
}
