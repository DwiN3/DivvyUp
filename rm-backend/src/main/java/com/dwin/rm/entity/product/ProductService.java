package com.dwin.rm.entity.product;


import com.dwin.rm.entity.person_product.PersonProduct;
import com.dwin.rm.entity.person_product.PersonProductRepository;
import com.dwin.rm.entity.person_product.PersonProductService;
import com.dwin.rm.entity.product.Request.AddProductRequest;
import com.dwin.rm.entity.receipt.ReceiptRepository;
import com.dwin.rm.security.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class ProductService {

}