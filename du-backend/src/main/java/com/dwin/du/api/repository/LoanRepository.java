package com.dwin.du.api.repository;
import com.dwin.du.api.entity.Loan;
import com.dwin.du.api.entity.Person;
import com.dwin.du.api.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.Date;
import java.util.List;

@Repository
public interface LoanRepository extends JpaRepository<Loan, Integer> {
    List<Loan> findByUser(User user);
    List<Loan> findByPerson(Person person);
    List<Loan> findByUserAndDateBetween(User user, Date dateFrom, Date dateTo);
}
