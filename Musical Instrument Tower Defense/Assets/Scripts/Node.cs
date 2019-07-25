using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;

    [Header("Optional")]
    public GameObject turret;

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition() {
        return transform.position + positionOffset;
    }

    void OnMouseDown()
    {
        // If the mouse is over UI element, return
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // If there's no selected turret in shop, return
        if (!buildManager.CanBuild)
            return;

        // If turret already exists on the node, return
        if (turret != null)
        {
            Debug.Log("Can't Build There! - TODO: Display on screen.");
            return;
        }

        buildManager.BuildTurretOn(this);
    }

    // Called first time, every time mouse enters the collider of object
    void OnMouseEnter()
    {
        // If the mouse is over UI element, return
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // If there's no selected turret in shop, return
        if (!buildManager.CanBuild)
            return;

        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
