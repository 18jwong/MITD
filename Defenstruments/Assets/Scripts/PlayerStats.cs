using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int startMoney = 1000;

    private static int money;
    private static int roundsSurvived;

    private void Awake()
    {
        money = startMoney;

        roundsSurvived = 0;
    }

    public static void AddMoney(int n)
    {
        money += n;
    }

    public static int GetMoney()
    {
        return money;
    }

    public static int GetRoundsSurvied()
    {
        return roundsSurvived;
    }
}
