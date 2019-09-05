using UnityEngine;

public class SpawnPointHolder : MonoBehaviour
{
    public static SpawnPointHolder instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one SpawnPointHolder in scene!");
            return;
        }
        instance = this;
    }

    public GameObject[] spawnPoints;
}
