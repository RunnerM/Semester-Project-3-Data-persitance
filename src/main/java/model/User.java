package model;

import java.util.ArrayList;

public class User {
    private int Id;
    private String username;
    private String password;
    private String email;
    private ArrayList<FriendsReference> friends;
    private ArrayList<SubscriptionReference> subscriptions;

    public User() {
    }

    public int getId() {
        return Id;
    }

    public void setId(int id) {
        Id = id;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public ArrayList<FriendsReference> getFriends() {
        return friends;
    }

    public void setFriends(ArrayList<FriendsReference> friends) {
        this.friends = friends;
    }

    public ArrayList<SubscriptionReference> getSubscriptions() {
        return subscriptions;
    }

    public void setSubscriptions(ArrayList<SubscriptionReference> subscriptions) {
        this.subscriptions = subscriptions;
    }
}
