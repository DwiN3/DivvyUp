package com.dwin.du.entity.person_item_share;

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
@Tag(name = "Person Item Share Management", description = "APIs for managing shares of item costs assigned to persons")
public class PersonItemShareController {

    private final PersonItemShareService personItemShareService;

    @PostMapping("/item/{itemId}/person-item-share/add")
    @Operation(summary = "Add a new person item share", description = "Adds a new share of an item's cost assigned to a person.")
    public ResponseEntity<?> add(@PathVariable int itemId, @RequestBody PersonItemShareDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personItemShareService.add(request, itemId, currentUsername);
    }

    @DeleteMapping("/person-item-share/remove/{id}")
    @Operation(summary = "Remove a person item share", description = "Removes the association of a person with a part of an item by its ID.")
    public ResponseEntity<?> remove(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personItemShareService.remove(id, currentUsername);
    }

    @PutMapping("/person-item-share/set-settled/{id}")
    @Operation(summary = "Set settled status", description = "Updates the settled status of a person's share of an item.")
    public ResponseEntity<?> setSettled(@PathVariable int id, @RequestBody PersonItemShareDto request){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personItemShareService.setIsSettled(id, request, currentUsername);
    }

    @PutMapping("/person-item-share/set-compensation/{id}")
    @Operation(summary = "Set compensation status", description = "Updates the compensation status of a person's share of an item.")
    public ResponseEntity<?> setCompensation(@PathVariable int id){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personItemShareService.setIsCompensation(id, currentUsername);
    }

    @GetMapping("/person-item-share/{id}")
    @Operation(summary = "Get person item share by ID", description = "Retrieves information about a person's share of an item by its ID.")
    public ResponseEntity<?> getPersonItemShare(@PathVariable int id) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personItemShareService.getPersonItemShare(id, currentUsername);
    }

    @GetMapping("/product/{itemId}/person-item-share")
    @Operation(summary = "Get all person item shares by item ID", description = "Retrieves all shares for a given item ID.")
    public ResponseEntity<?> getPersonItemShares(@PathVariable int itemId) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        String currentUsername = authentication.getName();
        return personItemShareService.getPersonItemsShare(itemId, currentUsername);
    }
}
