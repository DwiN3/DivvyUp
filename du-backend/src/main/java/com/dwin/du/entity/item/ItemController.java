package com.dwin.du.entity.item;

import com.dwin.du.entity.receipt.ReceiptDto;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rm")
@Tag(name = "Item Management", description = "APIs for managing items within receipts")
public class ItemController {

    private final ItemService itemService;


    @PostMapping("/receipt/{receiptID}/item/add")
    @Operation(summary = "Add a item to a receipt", description = "Adds a new item to a specific receipt.")
    public ResponseEntity<?> add(@PathVariable int receiptID, @RequestBody ItemDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return itemService.add(request, receiptID, currentUsername);
    }

    @PutMapping("/item/edit/{id}")
    @Operation(summary = "Edit a item", description = "Edits an existing item by ID.")
    public ResponseEntity<?> edit(@PathVariable int id, @RequestBody ReceiptDto request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return itemService.edit(id, request, currentUsername);
    }

    @DeleteMapping("/item/remove/{id}")
    @Operation(summary = "Remove a item", description = "Removes a item by ID.")
    public ResponseEntity<?> remove(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return itemService.remove(id, currentUsername);
    }


    @PutMapping("/item/set-settled/{id}")
    @Operation(summary = "Mark item as settled", description = "Marks a item as settled.")
    public ResponseEntity<?> setIsSettled(@PathVariable int id, @RequestBody ItemDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return itemService.setIsSettled(id, request, currentUsername);
    }

    @GetMapping("/item/{id}")
    @Operation(summary = "Get a item", description = "Retrieves a item by ID.")
    public ResponseEntity<?> getItem(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return itemService.getItemById(id, currentUsername);
    }

    @GetMapping("/receipt/{receiptID}/item")
    @Operation(summary = "Get all items in a receipt", description = "Retrieves all items within a specific receipt.")
    public ResponseEntity<?> getItems(@PathVariable int receiptID) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return itemService.getItemsFromReceipt(receiptID, currentUsername);
    }
}