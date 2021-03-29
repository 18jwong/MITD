using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TowerBuilder : NetworkBehaviour
{
    public static TowerBuilder instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("TowerBuilder: More than one TowerBuilder in scene!");
            return;
        }
        instance = this;
    }

    [SerializeField] private GameObject towerToBuildPrefab;

    [Server]
    public void SpawnTowerOnNode(GameObject nodeGO)
    {
        Node node = nodeGO.GetComponent<Node>();
        
        if(node.GetTower()) // if this node has a tower already, don't spawn one
        {
            Debug.Log("TowerBuilder: node has a tower already");
            return;
        }

        // spawn the tower
        GameObject tower = Instantiate(towerToBuildPrefab, nodeGO.transform.position, Quaternion.identity);
        NetworkServer.Spawn(tower);
        
        // set the node's tower to this tower
        node.SetTower(tower);
    }
}
