using UnityEngine;

public class Node : MonoBehaviour
{
    public SpriteRenderer rend;

    private Color originalColor;
    public Color secondColor;

    TowerBuilder towerBuilder;

    private GameObject tower = null;

    private void Start()
    {
        originalColor = rend.color;

        towerBuilder = TowerBuilder.instance;
    }

    private void OnMouseDown()
    {
        if(tower != null)
        {
            Debug.Log("Node: Tower exists here already");
            return;
        }

        // Build tower
        tower = towerBuilder.BuildTowerOnNode(this);
    }

    // Node Lightup functions -------------------------------

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
