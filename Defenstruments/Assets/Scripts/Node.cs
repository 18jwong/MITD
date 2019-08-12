using UnityEngine;

public class Node : MonoBehaviour
{
    public SpriteRenderer rend;

    private Color originalColor;
    public Color secondColor;

    TowerBuilder towerBuilder;
    TowerManager towerManager;

    private GameObject tower;

    // Unity Functions ------------------------------

    private void Start()
    {
        originalColor = rend.color;

        towerBuilder = TowerBuilder.instance;
        towerManager = TowerManager.instance;
    }

    private void OnMouseDown()
    {
        // Build tower
        Debug.Log("Tower Building");
        GameObject t = (GameObject)Instantiate(towerBuilder.GetTowerToBuild(), transform.position, Quaternion.identity);
        towerManager.AddTowerToTowers(t);
        tower = t;
    }

    private void OnMouseEnter()
    {
        towerBuilder.LightUpNodes(this);
    }

    private void OnMouseExit()
    {
        towerBuilder.UnlightUpNodes();
    }

    // Manipulation Functions ------------------------------

    public void LightUp()
    {
        rend.color = secondColor;
    }

    public void UnlightUp()
    {
        rend.color = originalColor;
    }
}
