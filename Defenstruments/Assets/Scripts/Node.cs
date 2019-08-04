using UnityEngine;

public class Node : MonoBehaviour
{
    public SpriteRenderer rend;
    public Color secondColor;
    private Color originalColor;

    public Grid grid;
    private TowerBuilder towerBuilder;


    // Unity Functions
    
    private void Start()
    {
        originalColor = rend.color;

        towerBuilder = grid.GetComponent<TowerBuilder>();
    }

    private void OnMouseEnter()
    {
        LightUp();
        towerBuilder.LightUpNodes(this);
    }

    private void OnMouseExit()
    {
        UnlightUp();
        towerBuilder.UnlightUpNodes();
    }

    // Manipulation Procedures

    public void LightUp()
    {
        rend.color = secondColor;
    }

    public void UnlightUp()
    {
        rend.color = originalColor;
    }
}
