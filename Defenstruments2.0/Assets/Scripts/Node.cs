using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class Node : NetworkBehaviour
{
    private GameObject tower;

    [Server]
    public GameObject GetTower()
    {
        return tower;
    }

    [Server]
    public void SetTower(GameObject towerGO)
    {
        RpcSetTower(towerGO);
    }
    [ClientRpc]
    private void RpcSetTower(GameObject towerGO)
    {
        tower = towerGO;
    }

}
