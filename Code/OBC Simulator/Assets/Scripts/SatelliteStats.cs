using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteStats : MonoBehaviour {

    [Header("Abilities of Satellite")]
    public bool commCollision = false;
    public bool lightCollision = false;

    [Header("Battery Options")]
    public static float battery = 100f;
    public float batteryDechargeRate = 0.1f;
    public float batteryRechargeRate = 0.1f;

    public float antennaDeployementRate = 0.5f;
    public float antennaDeployementTotalCost = 10f;

    public bool antennaDeployed = false;

    [Header("Unity Text Fields")]
    public Text batteryText;

    private void Start()
    {
        battery = 100f;
        commCollision = PlayerPrefsX.GetBool("commCollision");
        lightCollision = PlayerPrefsX.GetBool("lightCollision");

        batteryText.supportRichText = true;
    }

    private void Update()
    {
        if (GameManager.gameOver)
            return;

        commCollision = PlayerPrefsX.GetBool("commCollision");
        lightCollision = PlayerPrefsX.GetBool("lightCollision");

        BatteryUpdate();

    }

    void BatteryUpdate()
    {
        if (!lightCollision)
        {
            if (AntennaDeployer.deployAntenna && !antennaDeployed)
                AntennaDeployementCost(antennaDeployementTotalCost);

            battery -= batteryDechargeRate * Time.deltaTime;
        }
        else
        {
            battery += batteryRechargeRate * Time.deltaTime;
        }

        battery = Mathf.Clamp(battery, 0.00f, 100.0f);

        if (battery <= 33f)
            batteryText.color = Color.red;
        else if (battery <= 66f)
            batteryText.color = Color.yellow;
        else
            batteryText.color = Color.green;

        batteryText.supportRichText = true;

        batteryText.text = "Batteries: " + battery.ToString("0.00") + "%";
    }

    IEnumerator AntennaDeployementCost(float time)
    {
        float currentTime = 0.0f;

        do
        {
            battery -= antennaDeployementRate;
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }




}
