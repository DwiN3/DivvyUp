package com.dwin.du.entity.person_item_share;

import com.dwin.du.entity.person.Person;
import com.dwin.du.entity.person.PersonRepository;
import com.dwin.du.entity.item.Item;
import com.dwin.du.entity.item.ItemRepository;
import com.dwin.du.entity.user.User;
import com.dwin.du.entity.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class PersonItemShareService {

    private final UserRepository userRepository;
    private final PersonRepository personRepository;
    private final ItemRepository itemRepository;
    private final PersonItemShareRepository personItemShareRepository;

    public ResponseEntity<?> add(PersonItemShareDto request, int itemId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<Item> optionalItem = itemRepository.findById(itemId);
        if (!optionalItem.isPresent())
            return ResponseEntity.notFound().build();

        Item item = optionalItem.get();
        if (!item.getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<Person> optionalPerson = personRepository.findById(request.getPersonId());
        if (!optionalPerson.isPresent())
            return ResponseEntity.notFound().build();

        Person person = optionalPerson.get();
        if (!person.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<PersonItemShare> existingPersonItemShare = personItemShareRepository.findByItemAndPerson(item, person);
        if (existingPersonItemShare.isPresent()) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).build();
        }

        List<PersonItemShare> personItemShares = personItemShareRepository.findByItem(item);
        int currentTotalQuantity = personItemShares.stream()
                .mapToInt(PersonItemShare::getQuantity)
                .sum();

        if (currentTotalQuantity + request.getQuantity() > item.getMaxQuantity()) {
            return ResponseEntity.badRequest().build();
        }

        boolean isCompensation = personItemShares.isEmpty();

        PersonItemShare personItemShare = PersonItemShare.builder()
                .item(item)
                .person(person)
                .maxQuantity(item.getMaxQuantity())
                .isSettled(item.isSettled())
                .build();

        if (item.isDivisible()) {
            personItemShare.setQuantity(request.getQuantity());
            personItemShare.setPartOfPrice(request.getPartOfPrice());
        } else {
            personItemShare.setQuantity(1);
            personItemShare.setPartOfPrice(item.getPrice());
        }

        personItemShare.setCompensation(isCompensation);
        personItemShareRepository.save(personItemShare);

        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> remove(int id, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<PersonItemShare> optionalPersonItemShare = personItemShareRepository.findById(id);
        if (!optionalPersonItemShare.isPresent())
            return ResponseEntity.notFound().build();

        PersonItemShare personItemShare = optionalPersonItemShare.get();
        if (!personItemShare.getItem().getReceipt().getUser().getUsername().equals(username)) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        personItemShareRepository.delete(personItemShare);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsSettled(int id, PersonItemShareDto request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<PersonItemShare> optionalPersonItemShare = personItemShareRepository.findById(id);
        if (!optionalPersonItemShare.isPresent())
            return ResponseEntity.notFound().build();

        PersonItemShare personItemShare = optionalPersonItemShare.get();
        if (!personItemShare.getItem().getReceipt().getUser().getUsername().equals(username)) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        personItemShare.setSettled(request.isSettled());
        personItemShareRepository.save(personItemShare);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> setIsCompensation(int id, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<PersonItemShare> optionalPersonItemShare = personItemShareRepository.findById(id);
        if (!optionalPersonItemShare.isPresent())
            return ResponseEntity.notFound().build();

        PersonItemShare personItemShare = optionalPersonItemShare.get();
        if (!personItemShare.getItem().getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        List<PersonItemShare> personItemShares = personItemShareRepository.findByItem(personItemShare.getItem());

        for (PersonItemShare pp : personItemShares) {
            if (pp.getId() != id && pp.isCompensation()) {
                pp.setCompensation(false);
                personItemShareRepository.save(pp);
            }
        }

        personItemShare.setCompensation(true);
        personItemShareRepository.save(personItemShare);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getPersonItemShare(int id, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<PersonItemShare> optionalPersonItemShare = personItemShareRepository.findById(id);
        if (!optionalPersonItemShare.isPresent())
            return ResponseEntity.notFound().build();

        PersonItemShare personItemShare = optionalPersonItemShare.get();
        if (!personItemShare.getItem().getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        PersonItemShareDto response = PersonItemShareDto.builder()
                .id(personItemShare.getId())
                .itemId(personItemShare.getItem().getId())
                .personId(personItemShare.getPerson().getId())
                .partOfPrice(personItemShare.getPartOfPrice())
                .maxQuantity(personItemShare.getMaxQuantity())
                .quantity(personItemShare.getQuantity())
                .isCompensation(personItemShare.isCompensation())
                .isSettled(personItemShare.isSettled())
                .build();

        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getPersonItemsShare(int itemId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent())
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        Optional<Item> optionalItem = itemRepository.findById(itemId);
        if (!optionalItem.isPresent())
            return ResponseEntity.notFound().build();

        Item item = optionalItem.get();
        if (!item.getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        List<PersonItemShare> personItemShares = personItemShareRepository.findByItem(item);
        List<PersonItemShareDto> responseList = new ArrayList<>();
        for (PersonItemShare personItemShare : personItemShares) {
            PersonItemShareDto response = PersonItemShareDto.builder()
                    .id(personItemShare.getId())
                    .itemId(personItemShare.getItem().getId())
                    .personId(personItemShare.getPerson().getId())
                    .partOfPrice(personItemShare.getPartOfPrice())
                    .quantity(personItemShare.getQuantity())
                    .maxQuantity(personItemShare.getMaxQuantity())
                    .isCompensation(personItemShare.isCompensation())
                    .isSettled(personItemShare.isSettled())
                    .build();
            responseList.add(response);
        }

        return ResponseEntity.ok(responseList);
    }
}