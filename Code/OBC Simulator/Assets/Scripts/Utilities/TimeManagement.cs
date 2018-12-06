using UnityEngine;
using UnityEngine.UI;

/* Controls the speed of the game with the user buttons */
public class TimeManagement : MonoBehaviour {

    public Text speedText; // Speed text when on fast forward

    public float speedTime = 2f; // Start multiplier for fast forward

    private int status = 0; // Status of fast forward


    /* Set speed to normal level */
    private void Start()
    {
        Time.timeScale = 1f;
        speedText.text = "";
    }

    /* Speed the simulation and check fast forward status */ 
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

    /* Pause the simulation and reset status */
    public void PauseSpeed()
    {
        Time.timeScale = 0f;
        status = 0;
        speedText.text = "";
    }

    /* Resume the simulation and reset status */
    public void NormalSpeed()
    {
        Time.timeScale = 1f;
        status = 0;
        speedText.text = "";
    }
}
