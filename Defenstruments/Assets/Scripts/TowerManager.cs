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

                // Single Lane Targetting & the enemy is in the right row
                if (tower.targeting == TargetingMode.singleLane && (tower.GetRowNum() == rowNum))
                {
                    tower.AddEnemyInList(e);
                } // Adjacent Lane Targetting & enemy is in above, same, or below row
                else if(tower.targeting == TargetingMode.tripleLanes)
                {
                    bool above = tower.GetRowNum() + 1 == rowNum;
                    bool same = tower.GetRowNum() == rowNum;
                    bool below = tower.GetRowNum() - 1 == rowNum;
                    if (above || same || below)
                    {
                        tower.AddEnemyInList(e);
                    }
                } // All Lane Targetting
                else if (tower.targeting == TargetingMode.allLanes)
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

        // Add enemies to this tower for -single lane targeting-
        if (tower.targeting == TargetingMode.singleLane)
        {
            AddAllEnemiesInRowToTower(tower, tower.GetRowNum());
        } // triple lane targeting
        else if(tower.targeting == TargetingMode.tripleLanes)
        {
            // Add the below row if possible
            if(tower.GetRowNum()-1 >= 0)
            {
                AddAllEnemiesInRowToTower(tower, tower.GetRowNum()-1);
            }

            // Add the same row
            AddAllEnemiesInRowToTower(tower, tower.GetRowNum());

            // Add the above row if possible
            if(tower.GetRowNum()+1 < enemiesInRow.Count)
            {
                AddAllEnemiesInRowToTower(tower, tower.GetRowNum() + 1);
            }
        } // all lane targeting
        else if(tower.targeting == TargetingMode.allLanes)
        {
            LinkedListNode<LinkedList<GameObject>> rowNode = enemiesInRow.First;

            // Add enemies from every row
            for (int i = 0; i < enemiesInRow.Count; i++)
            {
                LinkedListNode<GameObject> cursor = rowNode.Value.First;
                // Add every enemy in current cursorInner's row
                for (int j = 0; j < rowNode.Value.Count; j++)
                {
                    tower.AddEnemyInList(cursor.Value);
                    cursor = cursor.Next;
                }
                rowNode = rowNode.Next;
            }
        }

    }
    private void RemoveTowerFromTowers(GameObject tGO)
    {
        Tower tower = tGO.GetComponent<Tower>();

        // Remove the tower from the towers linked list.
        NavigateToRow(towers, tower.GetRowNum()).Remove(tGO);

        // Remove enemies from this tower if -single lane targetting-
        if (tower.targeting == TargetingMode.singleLane)
        {
            RemoveAllEnemiesInRowFromTower(tower, tower.GetRowNum());
        }
        else if(tower.targeting == TargetingMode.tripleLanes)
        {
            // Remove the below row if possible
            if (tower.GetRowNum() - 1 >= 0)
            {
                RemoveAllEnemiesInRowFromTower(tower, tower.GetRowNum() - 1);
            }

            // Remove the same row
            RemoveAllEnemiesInRowFromTower(tower, tower.GetRowNum());

            // Remove the above row if possible
            if (tower.GetRowNum() + 1 < enemiesInRow.Count)
            {
                RemoveAllEnemiesInRowFromTower(tower, tower.GetRowNum() + 1);
            }
        }
        else if(tower.targeting == TargetingMode.allLanes)
        {
            LinkedListNode<LinkedList<GameObject>> rowNode = enemiesInRow.First;

            // Remove enemies from every row
            for (int i = 0; i < enemiesInRow.Count; i++)
            {
                LinkedListNode<GameObject> cursor = rowNode.Value.First;
                // Remove every enemy in current cursorInner's row
                for (int j = 0; j < rowNode.Value.Count; j++)
                {
                    tower.RemoveEnemyInList(cursor.Value);
                    cursor = cursor.Next;
                }
                rowNode = rowNode.Next;
            }
        }
    }

    // Helper function for AddTowerToTowers()
    private void AddAllEnemiesInRowToTower(Tower tower, int rowNum)
    {
        LinkedList<GameObject> row = NavigateToRow(enemiesInRow, rowNum);
        LinkedListNode<GameObject> cursor = row.First;

        // Add every enemy in current cursor's row
        for (int i = 0; i < row.Count; i++)
        {
            tower.AddEnemyInList(cursor.Value);
            cursor = cursor.Next;
        }
    }
    private void RemoveAllEnemiesInRowFromTower(Tower tower, int rowNum)
    {
        LinkedList<GameObject> row = NavigateToRow(enemiesInRow, rowNum);
        LinkedListNode<GameObject> cursor = row.First;

        // Remove every enemy in current cursor's row
        for (int i = 0; i < row.Count; i++)
        {
            tower.RemoveEnemyInList(cursor.Value);
            cursor = cursor.Next;
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
