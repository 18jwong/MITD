using UnityEngine;

public class Shop : MonoBehaviour
{
    public TowerBlueprint[] towerBlueprints;

    TowerBuilder towerBuilder;

    private void Start()
    {
        towerBuilder = TowerBuilder.instance;
    }

    // Select towers
    public void SelectTower(int i)
    {
        towerBuilder.SetTowerToBuild(towerBlueprints[i].prefab);
    }
}
