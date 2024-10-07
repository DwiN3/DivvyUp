package com.dwin.du.entity.person_product.Request;

public class ChangePersonRequest {
    private int personId;

    public ChangePersonRequest() {}

    public ChangePersonRequest(int personId) {
        this.personId = personId;
    }

    public int getPersonId() {
        return personId;
    }

    public void setPersonId(int personId) {
        this.personId = personId;
    }
}