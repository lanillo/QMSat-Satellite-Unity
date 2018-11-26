using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteStats : MonoBehaviour {

    [Header("Abilities of Satellite")]
    public bool commCollision = false;
    public bool lightCollision = false;
    public bool canTransmit = false;
    public bool canCapture = false;

    public static float battery = 100f;
    [Header("Battery Options")]
    public float batteryDechargeRate = 0.1f;
    public float batteryRechargeRate = 0.1f;
    [Space(10)]
    public float antennaDeployementRate = 1f;
    public float antennaDeployementTotalCost = 10f;
    private float cumulativeAntennaDeployment = 0f;
    private bool antennaDeployed = false;
    [Space(10)]
    public float batteryCaptureDataDechargeRate = 0.5f;
    [Space(10)]
    public float payloadDeployementRate = 1f;
    public float payloadDeployementTotalCost = 10f;
    private float cumulativePayloadDeployment = 0f;
    private bool payloadDeployed = false;

    [Header("Temperatures")]
    public float batteryTemperature = 60f;
    public float telecomTemperature = 60f;
    public float payloadTemperature = 60f;

    [Header("Player Stats")]
    public static float playerScore = 0;
    public static float playerMoney = 0;

    [Header("Unity Text Fields")]
    public Text batteryText;
    public Text batteryStatusText;
    public Text scoreText;
    public Text moneyText;

    [Header("Unity Objects")]
    public Gradient gr;
    public GameObject comm;
    public GameObject captureData;
    private Button commButton;
    private Button captureDataButton;

    //public bool test = false;

    private void Start()
    {
        battery = 100f;
        commCollision = PlayerPrefsX.GetBool("commCollision");
        lightCollision = PlayerPrefsX.GetBool("lightCollision");
        commButton = comm.GetComponent<Button>();
        captureDataButton = captureData.GetComponent<Button>();

        batteryText.supportRichText = true;
    }

    private void Update()
    {
        if (GameManager.gameOver)
            return;

        commCollision = PlayerPrefsX.GetBool("commCollision");
        lightCollision = PlayerPrefsX.GetBool("lightCollision");

        UpdateBattery();
        UpdatePlayerStats();
    }

    void UpdateBattery()
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

        // Check if payload is being deployed
        if (PayloadDeployer.deployPayload && !payloadDeployed)
        {
            cumulativePayloadDeployment += payloadDeployementRate * Time.deltaTime;
            battery -= payloadDeployementRate * Time.deltaTime;

            if (cumulativePayloadDeployment >= payloadDeployementTotalCost)
            {
                payloadDeployed = true;
            }
        }

        // If data is being captured
        if (CaptureData.capturingData)
        {
            battery -= batteryCaptureDataDechargeRate * Time.deltaTime;
        }

        // Check if you can transmit data to ground station (enable or disable button)
        CanTransmit();

        // Check if you can capture data from magnetometer (enable or disable button)
        CanCapture();

        // Check if it's on light
        if (!lightCollision)
        {
            battery -= batteryDechargeRate * Time.deltaTime;
            batteryStatusText.text = "Mode décharge";
        }
        else
        {
            battery += batteryRechargeRate * Time.deltaTime;
            batteryStatusText.text = "Mode chargement";
        }

        battery = Mathf.Clamp(battery, 0.00f, 100.0f);

        batteryText.color = gr.Evaluate(battery / 100f);
        batteryText.text = "Batteries: " + battery.ToString("0.00") + "%";

    }

    void CanTransmit()
    {
        // If antenna is not deployed, cannot trasnmit
        if (!antennaDeployed)
        {
            commButton.interactable = false;
            canTransmit = false;
        } //Check if you want them to communicate or not (hard mode ??)
        else if (commCollision)
        {
            commButton.interactable = true;
            canTransmit = true;
        }
        else
        {
            commButton.interactable = false;
            canTransmit = false;
        }
    }

    void CanCapture()
    {
        // If payload is not deployed, cannot capture data
        if (!payloadDeployed)
        {
            captureDataButton.interactable = false;
            canCapture = false;
        } 
        else
        {
            captureDataButton.interactable = true;
            canCapture = true;
        }
    }

    #region PlayerStats

    void UpdatePlayerStats()
    {
        //scoreText.text = "" //Show at GameOver
        moneyText.text = playerMoney.ToString() + " $  ";
    }

    public static void IncreaseScore(int amount)
    {
        playerScore += amount;
    }

    public static void IncreaseMoney(int amount)
    {
        playerMoney += amount;
    }

    #endregion PlayerStats
}
