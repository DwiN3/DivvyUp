package com.dwin.rm.entity.product;


import com.dwin.rm.entity.product.Request.AddProductRequest;
import com.dwin.rm.entity.product.Response.ShowProductResponse;
import com.dwin.rm.entity.receipt.Receipt;
import com.dwin.rm.entity.receipt.ReceiptRepository;
import com.dwin.rm.entity.receipt.Request.SetIsSettledRequest;
import com.dwin.rm.security.user.User;
import com.dwin.rm.security.user.UserRepository;
import io.jsonwebtoken.MalformedJwtException;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class ProductService {
    private final ProductRepository productRepository;
    private final ReceiptRepository receiptRepository;
    private final UserRepository userRepository;


}