package com.dwin.du.service;

import com.dwin.du.entity.person.PersonRepository;
import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.receipt.ReceiptRepository;
import com.dwin.du.valid.ValidService;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class OperationService {
    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;
    private final ValidService valid;

    public OperationService(ReceiptRepository receiptRepository, ProductRepository productRepository, PersonProductRepository personProductRepository, ValidService valid) {
        this.receiptRepository = receiptRepository;
        this.productRepository = productRepository;
        this.personProductRepository = personProductRepository;
        this.valid = valid;
    }

    public void updateTotalPriceReceipt(Receipt receipt) {
        List<Product> products = productRepository.findByReceipt(receipt);
        double totalPrice = 0.00;
        for(var item : products)
            totalPrice += item.getPrice();

        receipt.setTotalPrice(totalPrice);
        receiptRepository.save(receipt);
    }

    public void updatePartPricesPersonProduct(Product product) {
        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        for(var item : personProducts){
            double partPrice = calculatePartPrice(item.getQuantity(), product.getMaxQuantity(), product.getPrice());
            item.setPartOfPrice(partPrice);
            personProductRepository.save(item);
        }
    }

    public boolean areAllProductsSettled(Receipt receipt) {
        List<Product> products = productRepository.findByReceipt(receipt);
        return products.stream().allMatch(Product::isSettled);
    }


    public double calculatePartPrice(int quantity, int maxQuantity, double price) {
        double partPrice = ((double) quantity / maxQuantity) * price;
        return partPrice;
    }

    public double calculateCompensationPrice(List<PersonProduct> personProductDtos, double price){
        double compensationPrice = 0;
        for(var item : personProductDtos){
            compensationPrice += item.getPartOfPrice();
        }
        compensationPrice = price - compensationPrice;
        return compensationPrice;
    }

    public void updateCompensationPrice(Product product) {
        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        double compensationPrice = calculateCompensationPrice(personProducts, product.getPrice());
        product.setCompensationPrice(compensationPrice);
        productRepository.save(product);
    }

    public boolean areAllPersonProductsSettled(Product product) {
        List<PersonProduct> personProducts = personProductRepository.findByProduct(product);
        return personProducts.stream().allMatch(PersonProduct::isSettled);
    }
}
