package com.dwin.du.update;
import com.dwin.du.api.entity.*;
import com.dwin.du.api.repository.*;
import com.dwin.du.validation.ValidationService;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
@RequiredArgsConstructor
public class EntityUpdateService {
    private final PersonRepository personRepository;
    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;
    private final LoanRepository loanRepository;
    private final ValidationService validator;

    // Person
    public void updatePerson(String username, Boolean updateBalance) {
        User user = validator.validateUser(username);
        List<Person> persons = personRepository.findByUser(user);

        for (Person person : persons) {
            List<PersonProduct> personProducts = personProductRepository.findByPerson(person);

            long receiptsCount = personProducts.stream()
                    .map(personProduct -> personProduct.getProduct())
                    .filter(product -> product.getReceipt() != null)
                    .map(product -> product.getReceipt().getId())
                    .distinct()
                    .count();

            person.setReceiptsCount((int) receiptsCount);

            person.setProductsCount(personProducts.size());

            double totalAmount = personProducts.stream()
                    .mapToDouble(PersonProduct::getPartOfPrice)
                    .sum();
            person.setTotalAmount(totalAmount);

            double unpaidAmount = personProducts.stream()
                    .filter(personProduct -> !personProduct.isSettled())
                    .mapToDouble(PersonProduct::getPartOfPrice)
                    .sum();
            person.setUnpaidAmount(unpaidAmount);

            if(updateBalance){
                double balance = calculateBalance(person);
                person.setLoanBalance(balance);
            }
        }

        personRepository.saveAll(persons);
    }

    private double calculateBalance(Person person) {
        List<Loan> loans = loanRepository.findByPerson(person);
        double balance = 0.0;

        for (Loan loan : loans) {
            if(!loan.isSettled()){
                if (loan.isLent())
                    balance += loan.getAmount();
                else
                    balance -= loan.getAmount();
            }
        }

        return balance;
    }

    // Receipt
    public void updateTotalPriceReceipt(Receipt receipt) {
        List<Product> products = productRepository.findByReceipt(receipt);
        double totalPrice = 0.00;
        for(var item : products)
            totalPrice += item.getPrice();

        receipt.setTotalPrice(totalPrice);
        receiptRepository.save(receipt);
    }

    public boolean areAllProductsSettled(Receipt receipt) {
        List<Product> products = productRepository.findByReceipt(receipt);
        return products.stream().allMatch(Product::isSettled);
    }

    // Product
    public void updateCompensationPrice(Product product) {
        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        double compensationPrice = calculateCompensationPrice(personProducts, product.getPrice());
        product.setCompensationPrice(compensationPrice);
        productRepository.save(product);
    }
    public double calculateCompensationPrice(List<PersonProduct> personProductDtos, double price){
        double compensationPrice = 0;
        for(var item : personProductDtos){
            compensationPrice += item.getPartOfPrice();
        }
        compensationPrice = price - compensationPrice;
        return compensationPrice;
    }

    public boolean areAllPersonProductsSettled(Product product) {
        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        return personProducts.stream().allMatch(PersonProduct::isSettled);
    }

    // Person product
    public void updatePartPricesPersonProduct(Product product) {
        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        for(var item : personProducts){
            double partPrice = calculatePartPrice(item.getQuantity(), product.getMaxQuantity(), product.getPrice());
            item.setPartOfPrice(partPrice);
            personProductRepository.save(item);
        }
    }

    public double calculatePartPrice(int quantity, int maxQuantity, double price) {
        double partPrice = ((double) quantity / maxQuantity) * price;
        return partPrice;
    }
}
