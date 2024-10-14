package com.dwin.du.service;

import com.dwin.du.entity.person_product.PersonProduct;
import com.dwin.du.entity.person_product.PersonProductRepository;
import com.dwin.du.entity.product.Product;
import com.dwin.du.entity.product.ProductRepository;
import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.receipt.ReceiptRepository;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class DataUpdateService {
    private final ReceiptRepository receiptRepository;
    private final ProductRepository productRepository;
    private final PersonProductRepository personProductRepository;

    public DataUpdateService(ReceiptRepository receiptRepository, ProductRepository productRepository, PersonProductRepository personProductRepository) {
        this.receiptRepository = receiptRepository;
        this.productRepository = productRepository;
        this.personProductRepository = personProductRepository;
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
