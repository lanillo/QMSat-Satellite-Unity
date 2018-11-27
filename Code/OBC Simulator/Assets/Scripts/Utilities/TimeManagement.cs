using UnityEngine;

public class TimeManagement : MonoBehaviour {

    public float speedTime = 2f;

    private int status = 0;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void FastForwardSpeed()
    {
        if (status == 0)
        {
            Time.timeScale = speedTime;
            status++;
        }
        else
        {
            Time.timeScale = speedTime * 2;
            status = 0;
        }
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
