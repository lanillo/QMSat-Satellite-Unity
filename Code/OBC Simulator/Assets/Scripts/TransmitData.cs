using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransmitData : MonoBehaviour {

    public GameObject cooldownBar;
    private Slider slider;
    public Text CaptureDataText;

    List<MagnetometerData> dataset = new List<MagnetometerData>();

    public static bool transmittingData = false;
    public float timePerData = 0.5f;
    public float timeBuffer = 0f;
    private float timeBeforeNextTransmit = 0f;
    private int dataAmount = 0;
    private int dataTransmitted = 0;

    public float temperatureRate = 10f;

    public int PointsPerDataTransmitted = 100;

    private void Start()
    {
        cooldownBar.SetActive(false);
        slider = cooldownBar.GetComponentInChildren<Slider>();
        dataTransmitted = 0;
    }

    public void TransmitDataToGroundStation()
    {
        transmittingData = true;
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

        while (elapsedTime <= time && PlayerPrefsX.GetBool("commCollision"))
        {
            slider.value = elapsedTime / time;
            elapsedTime += Time.deltaTime;
            SatelliteStats.telecomTemperature += Time.deltaTime * temperatureRate;
            yield return null;
        }

        Debug.Log("Elapsed Time: " + elapsedTime.ToString() + " - TimePerData: " + timePerData.ToString());
        dataTransmitted = Mathf.FloorToInt(elapsedTime / timePerData);
        Debug.Log("Data Transmitted Successfully: " + Mathf.Round(dataTransmitted).ToString() + "/" + CaptureData.MAX_DATA_TO_SHARE.ToString());

        transmittingData = false;
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
        CaptureDataText.text = "Acquérir donnée (0/" + CaptureData.MAX_DATA_TO_SHARE.ToString() + ")";
    }

    public void UpdateScoreAndMoney()
    {
        SatelliteStats.IncreaseScore(dataTransmitted * PointsPerDataTransmitted);
        SatelliteStats.IncreaseMoney(dataTransmitted * PointsPerDataTransmitted);
    }
}
