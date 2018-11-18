using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteStats : MonoBehaviour {

    public float battery = 100f;
    public bool commCollision = false;
    public bool lightCollision = false;

    public Text batteryText;

    public float batteryDechargeRate = 0.1f;
    public float batteryRechargeRate = 0.1f;

    public GameObject GameOver;
    public bool satelliteDead = false;

    private void Start()
    {
        commCollision = PlayerPrefsX.GetBool("commCollision");
        lightCollision = PlayerPrefsX.GetBool("lightCollision");
        GameOver.SetActive(false);
    }

    private void Update()
    {
        if (battery <= 0f)
        {
            satelliteDead = true;
            GameOver.SetActive(true);
            batteryText.text = "";
        }

        if (satelliteDead)
            return;

        commCollision = PlayerPrefsX.GetBool("commCollision");
        lightCollision = PlayerPrefsX.GetBool("lightCollision");

        BatteryUpdate();

    }

    void BatteryUpdate()
    {
        if (!lightCollision)
        {
            battery -= batteryDechargeRate * Time.deltaTime;
        }
        else
        {
            battery += batteryRechargeRate * Time.deltaTime;
        }

        battery = Mathf.Clamp(battery, 0.00f, 100.0f);

        batteryText.text = "Battery: " + battery.ToString("0.00") + "%";
    }





}
