using UnityEngine;
using UnityEngine.EventSystems;

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
        // If the mouse is over UI element, return
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (tower != null)
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
        // If the mouse is over UI element, return
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        towerBuilder.LightUpNodes(this);
    }

    private void OnMouseExit()
    {
        // If the mouse is over UI element, return
        if (EventSystem.current.IsPointerOverGameObject())
            return;

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
