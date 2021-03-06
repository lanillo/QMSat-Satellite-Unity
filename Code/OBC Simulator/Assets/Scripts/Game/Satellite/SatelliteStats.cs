﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Contains everything related to the Satellite
 * Contains every recharge/decharge rate and increase/decrease rate for battery and temperature, respectively
 * Increases/decreases the temperature and increases/reduces battery according to sun position */
public class SatelliteStats : MonoBehaviour {

    [Header("Abilities of Satellite")]
    public bool commCollision = false;
    public bool lightCollision = false;
    public bool canTransmit = false;
    public bool canCapture = false;
    public bool enableHeater = false;

    [Header("Battery Options")]
    public float battery = 100f;
    public float batteryDechargeRate = 0.1f;
    public float batteryRechargeRate = 0.1f;
    public float batteryHeaterBatteryRate = 1.5f;
    [Space(5)]
    public float antennaDeployementRate = 1f;
    public float antennaDeployementTotalCost = 10f;
    private float cumulativeAntennaDeployment = 0f;
    private bool antennaDeployed = false;
    [Space(5)]
    public float payloadDeployementRate = 1f;
    public float payloadDeployementTotalCost = 10f;
    private float cumulativePayloadDeployment = 0f;
    private bool payloadDeployed = false;
    [Space(5)]
    public float captureDataDechargeRate = 0.5f;

    [Header("Temperatures")]
    public float temperatureBatteryIncreaseRate = 0.1f;
    public float temperatureTelecomIncreaseRate = 0.1f;
    public float temperaturePayloadIncreaseRate = 0.1f;
    public float temperatureBatteryDecreaseRate = 2.5f;
    public float temperatureTelecomDecreaseRate = 2.5f;
    public float temperaturePayloadDecreaseRate = 2.5f;
    public float batteryHeaterTemperatureRate = 1f;
    public bool batteryFailure = false;
    public bool telecomFailure = false;
    public bool payloadFailure = false;
    public float batteryTemperature = 60f;
    public float telecomTemperature = 60f;
    public float payloadTemperature = 60f;

    [Header("Player Stats")]
    public static float playerScore = 0;
    public static float playerMoney = 0;

    [Header("Unity Text Fields")]
    public Text batteryText;
    public Text scoreText;
    public Text moneyText;
    public Text alimText;
    public Text telecomText;
    public Text payloadText;

    [Header("Unity Objects")]
    public Gradient gr;
    public GameObject comm;
    public GameObject captureData;
    public Slider batterySlider;
    public Image batteryImage;
    private Button commButton;
    private Button captureDataButton;

    /* For Script accessing */
    private AntennaDeployer antennaDeployer;
    private PayloadDeployer payloadDeployer;

    private void Start()
    {
        // Start Values
        battery = 20f;
        playerMoney = 0f;
        playerScore = 0f;

        // Access Other Values
        antennaDeployer = GetComponent<AntennaDeployer>();
        payloadDeployer = GetComponent<PayloadDeployer>();

        // Get Fonctionnalities
        commButton = comm.GetComponent<Button>();
        captureDataButton = captureData.GetComponent<Button>();

        // Set Start Temperatures
        batteryTemperature = Random.Range(30f, 60f);
        payloadTemperature = Random.Range(10f, 50f);
        telecomTemperature = Random.Range(10, 50f);

        // Reset Bools
        batteryFailure = false;
        telecomFailure = false;
        payloadFailure = false;
        enableHeater = false;

        ReadTemperatures();
    }

    private void Update()
    {
        if (GameManager.gameOver)
            return;

        UpdateTemperature();
        UpdateBattery();
        UpdatePlayerStats();
    }

    void UpdateBattery()
    {
        // Check if antenna is being deployed
        if (antennaDeployer.deployAntenna && !antennaDeployed)
        {
            cumulativeAntennaDeployment += antennaDeployementRate * Time.deltaTime;
            battery -= antennaDeployementRate * Time.deltaTime;

            if (cumulativeAntennaDeployment >= antennaDeployementTotalCost)
                antennaDeployed = true;
        }

        // Check if payload is being deployed
        if (payloadDeployer.deployPayload && !payloadDeployed)
        {
            cumulativePayloadDeployment += payloadDeployementRate * Time.deltaTime;
            battery -= payloadDeployementRate * Time.deltaTime;

            if (cumulativePayloadDeployment >= payloadDeployementTotalCost)
                payloadDeployed = true;
        }

        // Check if you can transmit data to ground station (enable or disable button)
        CanTransmit();

        // Check if you can capture data from magnetometer (enable or disable button)
        CanCapture();

        // Check if it's on light
        if (!lightCollision)
            battery -= batteryDechargeRate * Time.deltaTime;
        else
            battery += batteryRechargeRate * Time.deltaTime;

        battery = Mathf.Clamp(battery, 0.00f, 100.0f);

        batterySlider.value = battery / 100f;
        batteryImage.color = Color.Lerp(Color.red, Color.green, batterySlider.value); 

        batteryText.color = gr.Evaluate(battery / 100f);
        batteryText.text = battery.ToString("0") + "%";
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
        scoreText.text = playerScore.ToString();
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

    #endregion

    #region Temperatures

    private void UpdateTemperature()
    {
        CheckTemperatureLimits();

        // Temperature updates from Light Collision
        if (lightCollision)
        {
            batteryTemperature += temperatureBatteryIncreaseRate * Time.deltaTime;
            payloadTemperature += temperaturePayloadIncreaseRate * Time.deltaTime;
            telecomTemperature += temperatureTelecomIncreaseRate * Time.deltaTime;
        }
        else
        {
            batteryTemperature -= temperatureBatteryDecreaseRate * Time.deltaTime;
            payloadTemperature -= temperaturePayloadDecreaseRate * Time.deltaTime;
            telecomTemperature -= temperatureTelecomDecreaseRate * Time.deltaTime;
        }

        // Enable Battery Heater
        if (enableHeater)
        {
            batteryTemperature += batteryHeaterTemperatureRate * Time.deltaTime;
            battery -= batteryHeaterBatteryRate * Time.deltaTime;
        }

        CheckTemperatureLimits();
    }

    void CheckTemperatureLimits()
    {
        if (batteryTemperature > 125f || batteryTemperature < -40f)
            batteryFailure = true;

        if (batteryTemperature < -40f || batteryTemperature > 125f )
            batteryFailure = true;

        if (payloadTemperature > 125f || payloadTemperature < -40f)
            payloadFailure = true;

        if (payloadTemperature < -40f || payloadTemperature > 125f)
            payloadFailure = true;

        if (telecomTemperature > 125f || telecomTemperature < -40f)
            telecomFailure = true;

        if (telecomTemperature < -40f || telecomTemperature > 125f)
            telecomFailure = true;
    }

    public void ReadAlimTemperature()
    {
        // -40 a 125;
        alimText.text = batteryTemperature.ToString("0") + " °C";
    }

    public void ReadPaylaodTemperature()
    {
        // -40 a 125;
        payloadText.text = payloadTemperature.ToString("0") + " °C";
    }

    public void ReadTelecomTemperature()
    {
        // -40 a 125;
        telecomText.text = telecomTemperature.ToString("0") + " °C";
    }

    public void ReadTemperatures()
    {
        ReadAlimTemperature();
        ReadPaylaodTemperature();
        ReadTelecomTemperature();
    }

    public void EnableHeater(bool input)
    {
        enableHeater = input;
    }

    #endregion
}
