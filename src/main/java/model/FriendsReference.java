package model;

import java.util.ArrayList;

public class FriendsReference {
    private ArrayList<User> friends;

    public FriendsReference() {
    }

    public ArrayList<User> getFriends() {
        return friends;
    }

    public void setFriends(ArrayList<User> friends) {
        this.friends = friends;
    }
}
