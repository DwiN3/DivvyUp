package com.dwin.rm.entity.person;

import com.dwin.rm.entity.person.Request.AddPersonRequest;
import com.dwin.rm.entity.person.Request.SetPersonReceiptsCountsRequest;
import com.dwin.rm.entity.person.Request.SetTotalAmountReceiptRequest;
import com.dwin.rm.entity.person.Response.ShowPersonResponse;
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
public class PersonService {

    private final PersonRepository personRepository;
    private final UserRepository userRepository;


}