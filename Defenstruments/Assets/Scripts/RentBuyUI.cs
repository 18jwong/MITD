using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RentBuyUI : MonoBehaviour
{
    public int uiHeight = 50;

    public GameObject rentBuyUIPanel;
    public GameObject rentButtonGO;
    public GameObject rentTextGO;
    public GameObject buyTextGO;
    private Button rentButton;
    private TextMeshProUGUI rentText;
    private TextMeshProUGUI buyText;

    public GameObject bottomPanel;
    private Transform[] buttonLocations;

    private int buttonIndex = -1;

    Shop shop;

    private void Start()
    {
        shop = Shop.instance;

        buttonLocations = new Transform[bottomPanel.transform.childCount];
        for (int i = 0; i < buttonLocations.Length; i++)
        {
            buttonLocations[i] = bottomPanel.transform.GetChild(i);
        }

        rentButton = rentButtonGO.GetComponent<Button>();
        rentText = rentTextGO.GetComponent<TextMeshProUGUI>();
        buyText = buyTextGO.GetComponent<TextMeshProUGUI>();
    }

    public void TogglePanel(int index)
    {
        // if the same button, toggle UI
        if (index == buttonIndex)
        {
            rentBuyUIPanel.SetActive(!rentBuyUIPanel.activeSelf);
            return;
        }
        // else if different, move UI

        // Set location
        Vector2 newLocation = buttonLocations[index].position;
        newLocation.y = newLocation.y + uiHeight;
        rentBuyUIPanel.transform.SetPositionAndRotation(newLocation, Quaternion.identity);

        // Set costs
        int rentCost = shop.GetTowerRentCost(index);
        if(rentCost == 0)
        {
            rentButton.interactable = false;
            rentText.text = "n/a";
        } else
        {
            rentButton.interactable = true;
            rentText.text = "$" + rentCost;
        }

        buyText.text = "$" + shop.GetTowerCost(index);

        rentBuyUIPanel.SetActive(true);

        buttonIndex = index;
    }

    public void RentButton()
    {
        // Set turretToBuild to be the rental version of the tower
        shop.SelectTower(buttonIndex);

        ClosePanel();
    }

    public void BuyButton()
    {
        // Set turretToBuild to be the standard version of the tower
        shop.SelectTower(buttonIndex);

        ClosePanel();
    }

    private void ClosePanel()
    {
        rentBuyUIPanel.SetActive(false);
    }
}
