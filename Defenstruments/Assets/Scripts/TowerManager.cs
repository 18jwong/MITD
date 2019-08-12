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
        LinkedListNode<LinkedList<GameObject>> cursor = enemiesInRow.First;
        for (int i = 0; i < rowNum; i++)
        {
            cursor = cursor.Next;
        }

        // Remove enemy from row 'rowNum' 
        cursor.Value.Remove(e);

        // Add enemy to towers
        RemoveEnemyFromTowers(e, rowNum);
    }


    private void AddEnemyToTowers(GameObject e, int rowNum)
    {
        LinkedListNode<GameObject> cursor = towers.First;
        for (int i = 0; i < towers.Count; i++)
        {
            Tower tower = cursor.Value.GetComponent<Tower>();

            if (!tower.singleLaneTargetting || (tower.GetRowNum() == rowNum))
            {
                tower.AddEnemy(e);
            }

            cursor = cursor.Next;
        }
    }
    private void RemoveEnemyFromTowers(GameObject e, int rowNum)
    {
        LinkedListNode<GameObject> cursor = towers.First;
        for (int i = 0; i < towers.Count; i++)
        {
            Tower tower = cursor.Value.GetComponent<Tower>();

            if (!tower.singleLaneTargetting || (tower.GetRowNum() == rowNum))
            {
                tower.RemoveEnemy(e);
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
            // Add enemies from every row
            for (int i = 0; i < enemiesInRow.Count; i++)
            {
                LinkedListNode<GameObject> cursorInner = cursorOuter.Value.First;
                // Add every enemy in current cursorInner's row
                for (int j = 0; j < cursorOuter.Value.Count; j++)
                {
                    tower.AddEnemy(cursorInner.Value);
                    cursorInner = cursorInner.Next;
                }
                cursorOuter = cursorOuter.Next;
            }
        } else
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
                tower.AddEnemy(cursorInner.Value);
                cursorInner = cursorInner.Next;
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
            // Remove enemies from every row
            for (int i = 0; i < enemiesInRow.Count; i++)
            {
                LinkedListNode<GameObject> cursorInner = cursorOuter.Value.First;
                // Add every enemy in current cursorInner's row
                for (int j = 0; j < cursorOuter.Value.Count; j++)
                {
                    tower.RemoveEnemy(cursorInner.Value);
                    cursorInner = cursorInner.Next;
                }
                cursorOuter = cursorOuter.Next;
            }
        }
        else
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
                tower.RemoveEnemy(cursorInner.Value);
                cursorInner = cursorInner.Next;
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
