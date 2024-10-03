package com.dwin.du.entity.item;

import com.dwin.du.entity.receipt.Receipt;
import com.dwin.du.entity.receipt.ReceiptDto;
import com.dwin.du.entity.receipt.ReceiptRepository;
import com.dwin.du.entity.user.User;
import com.dwin.du.entity.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
@RequiredArgsConstructor
public class ItemService {

    private final UserRepository userRepository;
    private final ReceiptRepository receiptRepository;
    private final ItemRepository itemRepository;

    public ResponseEntity<?> add(ItemDto request, int receiptId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();


        Item item = Item.builder()
                .receipt(receipt)
                .name(request.getName())
                .price(request.getPrice())
                .divisible(request.isDivisible())
                .maxQuantity(request.getMaxQuantity())
                .compensationPrice(0)
                .isSettled(receipt.isSettled())
                .build();

        if(item.isDivisible())
            item.setMaxQuantity(request.getMaxQuantity());
        else
            item.setMaxQuantity(1);

        itemRepository.save(item);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> edit(int itemId, ReceiptDto request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Item> optionalReceipt = itemRepository.findById(itemId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Item item = optionalReceipt.get();
        if (!item.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        item.setName(request.getName());
        itemRepository.save(item);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> remove(int itemId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Item> optionalItem = itemRepository.findById(itemId);
        if (!optionalItem.isPresent())
            return ResponseEntity.notFound().build();

        Item item = optionalItem.get();
        if (!item.getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        itemRepository.delete(item);

        return ResponseEntity.ok().build();
    }


    public ResponseEntity<?> setIsSettled(int itemId, ItemDto request, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Item> optionalItem = itemRepository.findById(itemId);
        if (!optionalItem.isPresent())
            return ResponseEntity.notFound().build();

        Item item = optionalItem.get();
        if (!item.getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        item.setSettled(request.isSettled());
        itemRepository.save(item);

        return ResponseEntity.ok().build();
    }

    public ResponseEntity<?> getItemById(int itemId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Item> optionalItem = itemRepository.findById(itemId);
        if (!optionalItem.isPresent())
            return ResponseEntity.notFound().build();

        Item item = optionalItem.get();
        if (!item.getReceipt().getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        ItemDto response = ItemDto.builder()
                .id(item.getId())
                .receiptId(item.getReceipt().getId())
                .name(item.getName())
                .price(item.getPrice())
                .compensationPrice(item.getCompensationPrice())
                .divisible(item.isDivisible())
                .maxQuantity(item.getMaxQuantity())
                .isSettled(item.isSettled())
                .build();

        return ResponseEntity.ok(response);
    }

    public ResponseEntity<?> getItemsFromReceipt(int receiptId, String username) {
        Optional<User> optionalUser = userRepository.findByUsername(username);
        if (!optionalUser.isPresent()) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }

        Optional<Receipt> optionalReceipt = receiptRepository.findById(receiptId);
        if (!optionalReceipt.isPresent())
            return ResponseEntity.notFound().build();

        Receipt receipt = optionalReceipt.get();
        if (!receipt.getUser().getUsername().equals(username))
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();

        List<Item> items = itemRepository.findByReceipt(optionalReceipt.get());

        List<ItemDto> responseList = new ArrayList<>();
        for (Item item : items) {
            ItemDto response = ItemDto.builder()
                    .id(item.getId())
                    .receiptId(item.getReceipt().getId())
                    .name(item.getName())
                    .price(item.getPrice())
                    .compensationPrice(item.getCompensationPrice())
                    .divisible(item.isDivisible())
                    .maxQuantity(item.getMaxQuantity())
                    .isSettled(item.isSettled())
                    .build();
            responseList.add(response);
        }

        return ResponseEntity.ok(responseList);
    }
}