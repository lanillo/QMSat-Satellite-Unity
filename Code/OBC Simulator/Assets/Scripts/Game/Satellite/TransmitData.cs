using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Controls where and when can data be transmitted 
 * Gives information on how many data was transmitted 
 * Increases score and money 
 * Increases the telecommunications temperature and reduces battery */
public class TransmitData : MonoBehaviour {

    [Header("Transmit Tweaks")]
    public float timePerData = 0.5f;
    public float timeBuffer = 0f;
    public float temperatureRate = 10f;
    public float batteryDechargeRate = 1.5f;
    public int PointsPerDataTransmitted = 100;

    [Header("Unity GameObjects")]
    public GameObject cooldownBar;
    public Text CaptureDataText;
    private Slider slider;

    private float timeBeforeNextTransmit = 0f;
    private int dataAmount = 0;
    private int dataTransmitted = 0;

    // For data memory
    List<MagnetometerData> dataset = new List<MagnetometerData>();

    /* Script Accessing */
    private SatelliteStats satelliteStats;
    private CaptureData captureData;

    private void Start()
    {
        // Get Satellite Stats Component
        satelliteStats = GetComponent<SatelliteStats>();
        captureData = GetComponent<CaptureData>();

        // Disable Cooldown Bar
        cooldownBar.SetActive(false);
        slider = cooldownBar.GetComponentInChildren<Slider>();
    }

    // Start Transmission
    public void TransmitDataToGroundStation()
    {
        dataAmount = CaptureData.GetIndex();
        Debug.Log("Data amount: " + dataAmount);
        timeBeforeNextTransmit = timePerData * dataAmount + timeBuffer;
        StartCoroutine(DelayBeforeTransmit(timeBeforeNextTransmit));
    }

    IEnumerator DelayBeforeTransmit(float time)
    {
        Debug.Log("Start of communication");
        dataTransmitted = 0;
        float elapsedTime = 0f;
        cooldownBar.SetActive(true);

        while (elapsedTime <= time && satelliteStats.commCollision)
        {
            slider.value = elapsedTime / time;
            elapsedTime += Time.deltaTime;
            satelliteStats.battery -= batteryDechargeRate * Time.deltaTime;
            satelliteStats.telecomTemperature += Time.deltaTime * temperatureRate;
            yield return null;
        }

        Debug.Log("Elapsed Time: " + elapsedTime.ToString() + " - TimePerData: " + timePerData.ToString());
        dataTransmitted = Mathf.FloorToInt(elapsedTime / timePerData);
        Debug.Log("Data Transmitted Successfully: " + Mathf.Round(dataTransmitted).ToString() + "/" + captureData.MAX_DATA.ToString());

        cooldownBar.SetActive(false);

        UpdateScoreAndMoney();
        ClearDataBuffer();
    }

    private void ClearDataBuffer()
    {
        for (int i = 0; i < dataTransmitted; i++)
        {
            dataset.Add(CaptureData.data[i]);
        }

        CaptureData.ResetIndex();
        CaptureDataText.text = "Acquisition de donnees (0/" + captureData.MAX_DATA.ToString() + ")";
    }

    public void UpdateScoreAndMoney()
    {
        SatelliteStats.IncreaseScore(dataTransmitted * PointsPerDataTransmitted);
        SatelliteStats.IncreaseMoney(dataTransmitted * PointsPerDataTransmitted);
    }
}
