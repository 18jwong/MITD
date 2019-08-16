using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;
    public float timeBtwnMoneyUpdates = 0.25f;

    void Start()
    {
        InvokeRepeating("UpdateMoney", 0f, timeBtwnMoneyUpdates);
    }

    void UpdateMoney()
    {
        moneyText.text = "$" + PlayerStats.GetMoney();
    }
}
