using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    private TowerBuilder towerBuilder;

    public override void OnStartClient()
    {
        towerBuilder = TowerBuilder.instance;
        
        base.OnStartClient();
    }

    [Client]
    void Update()
    {
        if(!isLocalPlayer) // if not the client, disable me
        {
            gameObject.SetActive(false);
            return;
        }

        if(Input.GetMouseButtonDown(0)) // left click
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if(!hit) return; // if nothing hit, don't do anything

            if (hit.collider.tag == "Node") {
                // try to spawn a tower
                CmdSpawnTowerOnNode(hit.collider.gameObject);
            }
        }
    }

    [Command]
    private void CmdSpawnTowerOnNode(GameObject nodeGO)
    {
        towerBuilder.SpawnTowerOnNode(nodeGO);
    }
}
