using System;

[Serializable]
public class UserClass
{
    public int score = 0, coins = 0;
    public int activeShip = -1;
}

[Serializable]
public class ShipClass
{
    public int id = -1;
    public string file, name;
    public int price = 0;
    public bool isUnlocked = false;
}