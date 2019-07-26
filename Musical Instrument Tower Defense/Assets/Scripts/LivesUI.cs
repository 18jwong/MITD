using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LivesUI : MonoBehaviour
{
    public Text livesText;
    public float timeBtwnLivesUpdates = 0.25f;

    private void Start()
    {
        StartCoroutine(UpdateLives());
    }

    // updateLives is called once every timeBtwnLivesUpdates second
    IEnumerator UpdateLives()
    {
        while (true) {
            livesText.text = PlayerStats.Lives + " LIVES";

            yield return new WaitForSeconds(timeBtwnLivesUpdates);
        }
    }
}
