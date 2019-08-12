using UnityEngine;

/* Notes: -need to set tower's rowNum after building for targetting to work
 * 
 */

public class TowerBuilder : MonoBehaviour
{
    public static TowerBuilder instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one TowerBuilder in scene!");
            return;
        }
        instance = this;
    }

    public Grid grid;
    public GameObject[] cols;

    private Node[] litNodes;
    private int nodeCount;

    private GameObject towerToBuild;

    // For TowerManager to create the correct number of rows
    public int GetRowCount()
    {
        return cols[0].transform.childCount;
    }

    // Lights up nodes vertically and horizontally from the node hovered over
    public void LightUpNodes(Node node)
    {
        // Determine cell location on grid
        Vector3 targetCell = grid.WorldToCell(node.transform.position);
        int cellX = (int)targetCell.x;
        int cellY = (int)targetCell.y;
        Transform column = cols[cellX].transform;

        // Save the nodes into array to unlight them
        litNodes = new Node[cols.Length + cols[0].transform.childCount - 1];
        nodeCount = 0;

        // Light up nodes vertically
        for (int i = 0; i < cols[0].transform.childCount; i++)
        {
            Transform t = column.GetChild(i);
            StoreNode(t);
        }

        // Light up nodes horizontally
        for (int i = 0; i < cols.Length; i++)
        {
            // So it doesn't double count the target
            if (i != cellX)
            {
                Transform t = cols[i].transform.GetChild(cellY);
                StoreNode(t);
            }
        }
    }

    // Helper function for LightUpNodes()
    private void StoreNode(Transform t)
    {
        litNodes[nodeCount] = t.GetComponent<Node>();
        litNodes[nodeCount].LightUp();
        nodeCount++;
    }

    // Unlights the stored nodes
    public void UnlightUpNodes()
    {
        for(int i = 0; i < litNodes.Length; i++)
        {
            litNodes[i].UnlightUp();
        }
    }

    public void SetTowerToBuild(GameObject t)
    {
        towerToBuild = t;
    }

    public GameObject GetTowerToBuild()
    {
        return towerToBuild;
    }
}
