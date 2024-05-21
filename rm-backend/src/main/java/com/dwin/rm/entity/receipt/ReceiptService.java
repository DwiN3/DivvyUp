package com.dwin.rm.entity.receipt;

import com.dwin.rm.entity.receipt.Request.AddReceiptRequest;
import com.dwin.rm.entity.receipt.Request.SetIsSettledRequest;
import com.dwin.rm.entity.receipt.Request.SetTotalAmountReceiptRequest;
import com.dwin.rm.entity.receipt.Response.ShowReceiptResponse;
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
public class ReceiptService {

    private final ReceiptRepository receiptRepository;
    private final UserRepository userRepository;

}
