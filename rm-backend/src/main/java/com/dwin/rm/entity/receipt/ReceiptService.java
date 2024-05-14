package com.dwin.rm.entity.receipt;

import com.dwin.rm.entity.person.Person;
import com.dwin.rm.entity.person.Request.AddPersonRequest;
import com.dwin.rm.entity.person_product.PersonProduct;
import com.dwin.rm.entity.person_product.PersonProductRepository;
import com.dwin.rm.entity.product.Product;
import com.dwin.rm.entity.product.ProductRepository;
import com.dwin.rm.entity.receipt.Request.AddReceiptRequest;
import com.dwin.rm.security.user.User;
import com.dwin.rm.security.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class ReceiptService {

    private final ReceiptRepository receiptRepository;
    private final UserRepository userRepository;

    
}
