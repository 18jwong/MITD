using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one TowerManager in scene!");
            return;
        }
        instance = this;
    }

    TowerBuilder towerBuilder;

    private LinkedList<LinkedList<GameObject>> towers = new LinkedList<LinkedList<GameObject>>();
    private LinkedList<LinkedList<GameObject>> enemiesInRow = new LinkedList<LinkedList<GameObject>>();

    private void Start()
    {
        towerBuilder = TowerBuilder.instance;

        // Creates a LinkedList row for each row on screen
        // corresponding with y coord, bottom to top
        for (int i = 0; i < towerBuilder.GetRowCount(); i++)
        {
            towers.AddLast(new LinkedList<GameObject>());
        }
        for (int i = 0; i < towerBuilder.GetRowCount(); i++)
        {
            enemiesInRow.AddLast(new LinkedList<GameObject>());
        }
    }

    // Tower Manager -----------------------------------
    // Note: First row is 0, NOT 1.
    public void AddEnemy(GameObject e, int rowNum)
    {
        // Navigate to correct row
        // Add enemy to row 'rowNum' 
        NavigateToRow(enemiesInRow, rowNum).AddLast(e);

        // Add enemy to towers
        AddEnemyToTowers(e, rowNum);

        // Add towers to enemy
        AddTowersToEnemy(e);
    }
    public void RemoveEnemy(GameObject e)
    {
        // Remove enemy From towers
        RemoveEnemyFromTowers(e);

        // Add enemies to the enemies LinkedList
        RemoveEnemyFromEnemies(e);
    }

    private void RemoveEnemyFromEnemies(GameObject e)
    {
        Enemy enemy = e.GetComponent<Enemy>();

        // Navigate to correct row
        LinkedList<GameObject> row = NavigateToRow(enemiesInRow, enemy.GetRowNum());

        // Remove enemy from row 'rowNum' 
        LinkedListNode<GameObject> eNode = row.First;
        for (int i = 0; i < row.Count; i++)
        {
            if (eNode.Value == e)
            {
                row.Remove(eNode);
                return;
            }

            eNode = eNode.Next;
        }

        // If the enemy is not found, then we have an issue
        Debug.Log("TowerManager error: enemy not removed...\n\tRow's enemies: " + row.ToString());
    }

    // Helper functions for Adding/Removing enemies
    private void AddEnemyToTowers(GameObject e, int rowNum)
    {
        LinkedListNode<LinkedList<GameObject>> rowCursor = towers.First;
        for (int i = 0; i < towers.Count; i++)
        {
            LinkedList<GameObject> row = rowCursor.Value;
            LinkedListNode<GameObject> cursor = row.First;
            for (int j = 0; j < row.Count; j++)
            {
                Tower tower = cursor.Value.GetComponent<Tower>();

                if (!tower.singleLaneTargetting || (tower.GetRowNum() == rowNum))
                {
                    tower.AddEnemyInList(e);
                }

                cursor = cursor.Next;
            }

            rowCursor = rowCursor.Next;
        }
    }
    private void RemoveEnemyFromTowers(GameObject e)
    {
        LinkedListNode<LinkedList<GameObject>> rowCursor = towers.First;
        for (int i = 0; i < towers.Count; i++)
        {
            LinkedList<GameObject> row = rowCursor.Value;
            LinkedListNode<GameObject> cursor = row.First;
            for (int j = 0; j < row.Count; j++)
            {
                Tower tower = cursor.Value.GetComponent<Tower>();

                if (tower.ContainsEnemyInList(e))
                {
                    tower.RemoveEnemyInList(e);
                }

                cursor = cursor.Next;
            }

            rowCursor = rowCursor.Next;
        }
    }

    // 'Enemy' Manager -----------------------------------

    public void AddTower(GameObject tGO, int rowNum)
    {
        AddTowerToTowers(tGO, rowNum);
        AddTowerToEnemies(tGO);
    }
    public void RemoveTower(GameObject tGO)
    {
        RemoveTowerFromTowers(tGO);
        RemoveTowerFromEnemies(tGO);
    }

    private void AddTowerToTowers(GameObject tGO, int rowNum)
    {
        NavigateToRow(towers, rowNum).AddLast(tGO);

        Tower tower = tGO.GetComponent<Tower>();
        LinkedListNode<LinkedList<GameObject>> cursorOuter = enemiesInRow.First;

        // Add enemies to this tower
        if (tower.singleLaneTargetting)
        {
            // Navigate to correct row
            for (int i = 0; i < tower.GetRowNum(); i++)
            {
                cursorOuter = cursorOuter.Next;
            }

            LinkedListNode<GameObject> cursorInner = cursorOuter.Value.First;
            // Add every enemy in current cursor's row
            for (int i = 0; i < cursorOuter.Value.Count; i++)
            {
                tower.AddEnemyInList(cursorInner.Value);
                cursorInner = cursorInner.Next;
            }
        } else
        {
            // Add enemies from every row
            for (int i = 0; i < enemiesInRow.Count; i++)
            {
                LinkedListNode<GameObject> cursorInner = cursorOuter.Value.First;
                // Add every enemy in current cursorInner's row
                for (int j = 0; j < cursorOuter.Value.Count; j++)
                {
                    tower.AddEnemyInList(cursorInner.Value);
                    cursorInner = cursorInner.Next;
                }
                cursorOuter = cursorOuter.Next;
            }
        }
    }
    private void RemoveTowerFromTowers(GameObject tGO)
    {
        Tower tower = tGO.GetComponent<Tower>();

        // Remove the tower from the towers linked list.
        NavigateToRow(towers, tower.GetRowNum()).Remove(tGO);

        LinkedListNode<LinkedList<GameObject>> cursorOuter = enemiesInRow.First;

        // Remove enemies from this tower
        if (tower.singleLaneTargetting)
        {
            // Navigate to correct row
            for (int i = 0; i < tower.GetRowNum(); i++)
            {
                cursorOuter = cursorOuter.Next;
            }

            LinkedListNode<GameObject> cursorInner = cursorOuter.Value.First;
            // Remove every enemy in current cursor's row
            for (int i = 0; i < cursorOuter.Value.Count; i++)
            {
                tower.RemoveEnemyInList(cursorInner.Value);
                cursorInner = cursorInner.Next;
            }
        }
        else
        {
            // Remove enemies from every row
            for (int i = 0; i < enemiesInRow.Count; i++)
            {
                LinkedListNode<GameObject> cursorInner = cursorOuter.Value.First;
                // Remove every enemy in current cursorInner's row
                for (int j = 0; j < cursorOuter.Value.Count; j++)
                {
                    tower.RemoveEnemyInList(cursorInner.Value);
                    cursorInner = cursorInner.Next;
                }
                cursorOuter = cursorOuter.Next;
            }
        }
    }

    // 'Enemy' Manager helper functions

    // Should be called when adding an enemy to the scene. 
    // (Called by AddEnemy() above)
    private void AddTowersToEnemy(GameObject enemyGO)
    {
        Enemy enemy = enemyGO.GetComponent<Enemy>();

        // Navigate to correct row
        LinkedList<GameObject> row = NavigateToRow(towers, enemy.GetRowNum());

        // Add every tower in that row to the enemy's hit list.
        LinkedListNode<GameObject> cursor = row.First;
        for (int i = 0; i < row.Count; i++)
        {
            enemy.AddTowerInList(cursor.Value);
            
            cursor = cursor.Next;
        }
    }

    // Should be called when adding a tower to the scene.
    // (Called by AddTower() above)
    private void AddTowerToEnemies(GameObject towerGO)
    {
        Tower tower = towerGO.GetComponent<Tower>();

        // Navigate to correct row
        LinkedList<GameObject> row = NavigateToRow(enemiesInRow, tower.GetRowNum());

        // Add tower to every enemy-in-that-row's hit list.
        LinkedListNode<GameObject> cursor = row.First;
        for (int i = 0; i < row.Count; i++)
        {
            cursor.Value.GetComponent<Enemy>().AddTowerInList(towerGO);

            cursor = cursor.Next;
        }
    }

    // Should be called when 1. tower dies or 2. tower is removed.
    // (Called by RemoveTower() above)
    private void RemoveTowerFromEnemies(GameObject towerGO)
    {
        Tower tower = towerGO.GetComponent<Tower>();

        // Navigate to correct row
        LinkedList<GameObject> row = NavigateToRow(enemiesInRow, tower.GetRowNum());

        // Remove tower from every enemy-in-that-row's hit list.
        LinkedListNode<GameObject> cursor = row.First;
        for (int i = 0; i < row.Count; i++)
        {
            cursor.Value.GetComponent<Enemy>().RemoveTowerInList(towerGO);

            cursor = cursor.Next;
        }
    }

    // Other Functions ------------------------------------

    // Helper function to navigate to the correct LinkedListNode / row.
    public LinkedList<GameObject> NavigateToRow(LinkedList<LinkedList<GameObject>> rows, int rowNum)
    {
        LinkedListNode<LinkedList<GameObject>> rowNode = rows.First;
        for (int i = 0; i < rowNum; i++)
        {
            rowNode = rowNode.Next;
        }

        return rowNode.Value;
    }

    public LinkedList<LinkedList<GameObject>> GetEnemiesList()
    {
        return enemiesInRow;
    }

}
