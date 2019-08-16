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

    private LinkedList<GameObject> towers = new LinkedList<GameObject>();
    private LinkedList<LinkedList<GameObject>> enemiesInRow = new LinkedList<LinkedList<GameObject>>();

    private void Start()
    {
        towerBuilder = TowerBuilder.instance;

        // Creates a LinkedList row for each row on screen
        // corresponding with y coord, bottom to top
        for (int i = 0; i < towerBuilder.GetRowCount(); i++)
        {
            enemiesInRow.AddLast(new LinkedList<GameObject>());
        }
    }

    // Note: First row is 0, NOT 1.
    public void AddEnemy(GameObject e, int rowNum)
    {
        // Navigate to correct row
        LinkedListNode<LinkedList<GameObject>> cursor = enemiesInRow.First;
        for (int i = 0; i < rowNum; i++)
        {
            cursor = cursor.Next;
        }
        
        // Add enemy to row 'rowNum' 
        cursor.Value.AddLast(e);

        // Add enemy to towers
        AddEnemyToTowers(e, rowNum);
    }
    public void RemoveEnemy(GameObject e, int rowNum)
    {
        // Remove enemy From towers
        RemoveEnemyFromTowers(e);

        // Navigate to correct row
        LinkedListNode<LinkedList<GameObject>> cursor = enemiesInRow.First;
        for (int i = 0; i < rowNum; i++)
        {
            cursor = cursor.Next;
        }
        LinkedList<GameObject> row = cursor.Value;

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


    private void AddEnemyToTowers(GameObject e, int rowNum)
    {
        LinkedListNode<GameObject> cursor = towers.First;
        for (int i = 0; i < towers.Count; i++)
        {
            Tower tower = cursor.Value.GetComponent<Tower>();

            if (!tower.singleLaneTargetting || (tower.GetRowNum() == rowNum))
            {
                tower.AddEnemyInList(e);
            }

            cursor = cursor.Next;
        }
    }
    private void RemoveEnemyFromTowers(GameObject e)
    {
        LinkedListNode<GameObject> cursor = towers.First;
        for (int i = 0; i < towers.Count; i++)
        {
            Tower tower = cursor.Value.GetComponent<Tower>();

            if (tower.ContainsEnemyInList(e))
            {
                tower.RemoveEnemyInList(e);
            }

            cursor = cursor.Next;
        }
    }

    public void AddTowerToTowers(GameObject tGO)
    {
        towers.AddLast(tGO);

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
    public void RemoveTowerFromTowers(GameObject tGO)
    {
        towers.Remove(tGO);

        Tower tower = tGO.GetComponent<Tower>();
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

    /*
    public void AddEnemyToEnemies(GameObject e)
    {
        enemiesInRow.AddLast(e);
    }
    public void RemoveEnemyFromEnemies(GameObject e)
    {
        enemiesInRow.Remove(e);
    }
    */
}
