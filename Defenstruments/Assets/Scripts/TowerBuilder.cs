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

    TowerManager towerManager;

    public Grid grid;
    public GameObject[] cols;

    private Node[] litNodes;
    private int nodeCount;

    private TowerBlueprint towerToBuild;

    private void Start()
    {
        towerManager = TowerManager.instance;
    }

    // Node Lightup functions ------------------------------------------------

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
            if(litNodes[i] == null)
            {
                Debug.LogError("TowerBuilder: node not found");
                return;
            }

            litNodes[i].UnlightUp();
        }
    }

    // Build Tower functions ------------------------------------------------

    // Builds a tower on the given node and returns a reference to it
    public GameObject BuildTowerOnNode(Node node)
    {
        // If no tower is selected, return null
        if (towerToBuild == null)
        {
            Debug.Log("TowerBuilder: No tower selected");
            return null;
        }

        // If not enough money, return null
        if(PlayerStats.GetMoney() - towerToBuild.cost < 0)
        {
            Debug.Log("TowerBuilder: Not enough money");
            return null;
        }

        // Subtract money
        PlayerStats.AddMoney(-towerToBuild.cost);

        // Create tower
        GameObject t = (GameObject)Instantiate(towerToBuild.prefab, node.transform.position, Quaternion.identity);

        // Set row number
        int rowNum = GetNodeRowNumber(node);
        t.GetComponent<Tower>().SetRowNum(rowNum);

        // Set up targetting
        towerManager.AddTower(t, rowNum);

        // Reset towerToBuild
        towerToBuild = null;

        return t;
    }

    // Store the selected tower in shop
    public void SetTowerToBuild(TowerBlueprint t)
    {
        towerToBuild = t;
    }

    // Returns the selected tower in shop
    /*
    public GameObject GetTowerToBuild()
    {
        return towerToBuild;
    }
    */

    // For TowerManager to create the correct number of rows
    public int GetRowCount()
    {
        return cols[0].transform.childCount;
    }

    // Returns the row number of the node passed in
    public int GetNodeRowNumber(Node node)
    {
        Vector3 targetCell = grid.WorldToCell(node.transform.position);
        return (int)targetCell.y;
    }
}
