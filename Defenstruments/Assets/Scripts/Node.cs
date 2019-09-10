using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public SpriteRenderer rend;

    private Color originalColor;
    public Color secondColor;
    public Color cannotBuildColor;

    public float checkTime = 0.25f;

    TowerBuilder towerBuilder;

    private GameObject tower = null;
    private GameObject towerGhost = null;

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

        if (tower != null && towerGhost != null)
        {
            Destroy(towerGhost);
        }
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

    public void LightUpCursorNode(TowerBlueprint towerToBuild)
    {
        // If there's no tower, instantiate the ghost version of the tower
        if(tower == null)
        {
            towerGhost = (GameObject)Instantiate(towerToBuild.prefabGhost, this.transform.position, Quaternion.identity);

            // If not enough money, color the node red
            StartCoroutine(CheckMoney(towerToBuild));

        } // Else there's a tower there already so color the target node the cannotBuildColor
        else
        {
            rend.color = cannotBuildColor;
        }
    }

    // If not enough money, color the node red
    IEnumerator CheckMoney(TowerBlueprint towerToBuild)
    {
        while (towerGhost != null) {
            if (PlayerStats.GetMoney() - towerToBuild.cost < 0)
            {
                rend.color = cannotBuildColor;
            } else
            {
                rend.color = secondColor;
            }

            yield return new WaitForSeconds(checkTime);
        }
    }

    public void UnlightUp()
    {
        rend.color = originalColor;
        
        if(towerGhost != null)
        {
            Destroy(towerGhost);
        }
    }
}
