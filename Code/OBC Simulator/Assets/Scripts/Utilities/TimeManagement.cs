using UnityEngine;
using UnityEngine.UI;

public class TimeManagement : MonoBehaviour {

    public Text speedText; 

    public float speedTime = 2f;

    private int status = 0;

    private void Start()
    {
        Time.timeScale = 1f;
        speedText.text = "";
    }

    public void FastForwardSpeed()
    {
        if (status == 0)
        {
            Time.timeScale = speedTime;
            status++;
            speedText.text = "X" + speedTime.ToString();
        }
        else
        {
            Time.timeScale = speedTime * 2;
            speedText.text = "X" + (2 *speedTime).ToString();
            status = 0;
        }
    }

    public void PauseSpeed()
    {
        Time.timeScale = 0f;
        speedText.text = "";
    }

    public void NormalSpeed()
    {
        Time.timeScale = 1f;
        speedText.text = "";
    }
}
