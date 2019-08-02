using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;
    public float timeBtwnMoneyUpdates = 0.25f;

    private void Start()
    {
        StartCoroutine(UpdateMoney());
    }

    // UpdateMoney is called once every timeBtwnMoneyUpdates seconds
    IEnumerator UpdateMoney()
    {
        while (true)
        {
            moneyText.text = "$" + PlayerStats.Money.ToString();

            yield return new WaitForSeconds(timeBtwnMoneyUpdates);
        }
    }
}
