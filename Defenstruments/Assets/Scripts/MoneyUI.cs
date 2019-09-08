using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;
    public float timeBtwnMoneyUpdates = 0.25f;

    public GameObject bottomPanel;

    Shop shop;

    void Start()
    {
        shop = Shop.instance;

        if(bottomPanel.transform.childCount != shop.towerBlueprintGOs.Length)
        {
            Debug.LogError("MoneyUI: # of blueprints don't match the # of buttons in bottom panel.");
        }
        else
        {
            // Set button prices for each tower
            for (int i = 0; i < shop.towerBlueprintGOs.Length; i++)
            {
                Text t = bottomPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>();
                t.text = "$" + shop.GetTowerCost(i);
            }
        }

        InvokeRepeating("UpdateMoney", 0f, timeBtwnMoneyUpdates);
    }

    void UpdateMoney()
    {
        moneyText.text = "$" + PlayerStats.GetMoney();
    }
}
