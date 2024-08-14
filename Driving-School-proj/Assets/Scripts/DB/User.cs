using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string username;
    public int gold;


    public User(string username, int gold) {
        this.username = username;
        this.gold = gold;
    }
}
