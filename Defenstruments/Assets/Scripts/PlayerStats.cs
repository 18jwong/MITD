using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int startMoney = 1000;

    private static int money;

    private void Awake()
    {
        money = startMoney;
    }

    public static void AddMoney(int n)
    {
        money += n;
    }

    public static int GetMoney()
    {
        return money;
    }
}
