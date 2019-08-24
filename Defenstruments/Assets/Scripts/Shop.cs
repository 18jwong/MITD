using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Shop in scene!");
            return;
        }
        instance = this;
    }

    public GameObject[] towerBlueprintGOs;

    TowerBuilder towerBuilder;

    private void Start()
    {
        towerBuilder = TowerBuilder.instance;
    }

    // Select towers
    public void SelectTower(int i)
    {
        towerBuilder.SetTowerToBuild(towerBlueprintGOs[i].GetComponent<TowerBlueprintHolder>().towerBlueprint);
    }

    // Return the tower cost
    public int GetTowerCost(GameObject tBGO)
    {
        return tBGO.GetComponent<TowerBlueprintHolder>().towerBlueprint.cost;
    }
}
