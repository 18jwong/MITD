using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public GameObject[] towers; // TODO: make this private

    // TODO: so we can add enemies to towers that are built
    //private GameObject[] enemiesInRow;

    public void AddEnemyToTowers(GameObject e)
    {
        for (int i = 0; i < towers.Length; i++)
        {
            towers[i].GetComponent<Tower>().AddEnemy(e);
        }
    }

    public void RemoveEnemyFromTowers(GameObject e)
    {
        for (int i = 0; i < towers.Length; i++)
        {
            towers[i].GetComponent<Tower>().RemoveEnemy(e);
        }
    }

    // TODO: add function for adding tower to towers[]
}
