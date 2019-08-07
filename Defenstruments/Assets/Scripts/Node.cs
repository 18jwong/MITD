using UnityEngine;

public class Node : MonoBehaviour
{
    public SpriteRenderer rend;

    private Color originalColor;
    public Color secondColor;

    public Grid grid;
    private TowerBuilder towerBuilder;

    // Unity Functions ------------------------------

    private void Start()
    {
        originalColor = rend.color;

        towerBuilder = grid.GetComponent<TowerBuilder>();
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
