using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject[] towerBlueprints;

    TowerBuilder towerBuilder;

    private void Start()
    {
        towerBuilder = TowerBuilder.instance;
    }

    // Select towers
    public void SelectTower(int i)
    {
        towerBuilder.SetTowerToBuild(towerBlueprints[i].GetComponent<TowerBlueprintHolder>().towerBlueprint);
    }
}
