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
    
    static public float cumulativeAntennaDeployment = 0f;
    public float antennaDeployementRate = 1f;
    public float antennaDeployementTotalCost = 10f;

    public bool antennaDeployed = false;

    [Header("Unity Text Fields")]
    public Text batteryText;

    public Gradient gr;

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

        // Check if antenna is being deployed
        if (AntennaDeployer.deployAntenna && !antennaDeployed)
        {
            cumulativeAntennaDeployment += antennaDeployementRate * Time.deltaTime;
            battery -= antennaDeployementRate * Time.deltaTime;

            if (cumulativeAntennaDeployment >= antennaDeployementTotalCost)
            {
                antennaDeployed = true;
            }
        }

        // Check if it's on light
        if (!lightCollision)
        {
            battery -= batteryDechargeRate * Time.deltaTime;
        }
        else
        {
            battery += batteryRechargeRate * Time.deltaTime;
        }

        battery = Mathf.Clamp(battery, 0.00f, 100.0f);

        batteryText.color = gr.Evaluate(battery / 100f);
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
