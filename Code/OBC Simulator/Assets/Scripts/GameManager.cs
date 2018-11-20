using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Unity UI Objects")]
    public GameObject gameUI;
    public GameObject gameOverUI;

    public static bool gameOver = false;

    private void Start()
    {
        gameOver = false;
        gameOverUI.SetActive(false);
        gameUI.SetActive(true);
    }

    private void Update()
    {
        if (SatelliteStats.battery <= 0f)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameOver = true;
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);
    }
}
