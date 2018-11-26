using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour {

    public float speedTime = 2f;

    public void FastForwardSpeed()
    {
        Time.timeScale = speedTime;
    }

    public void PauseSpeed()
    {
        Time.timeScale = 0f;
    }

    public void NormalSpeed()
    {
        Time.timeScale = 1f;
    }
}
