using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CaptureData : MonoBehaviour
{
    [Header("Capture Tweaks")]
    public float timeBeforeNextCapture = 5f;
    public float batteryDechargeRate = 0.5f;
    public float temperatureRate = 1.5f;
    public int MAX_DATA = 15;

    [Header("Unity Objects")]
    public Transform magnetometer;
    public GameObject cooldownBar;
    public Gradient gr;
    public Text CaptureDataText;
    public static MagnetometerData[] data;
    private Slider slider;
    private SatelliteStats satelliteStats;

    [Header("Capture Status")]
    public bool capturingData = false;

    private static int index = 0;

    private void Start()
    {
        data = new MagnetometerData[MAX_DATA];
        satelliteStats = GetComponent<SatelliteStats>();
        index = 0;
        capturingData = false;
        cooldownBar.SetActive(false);
        slider = cooldownBar.GetComponentInChildren<Slider>();
        CaptureDataText.text = "Acquisition de donnees (" + GetIndex().ToString() + "/" + MAX_DATA.ToString() + ")";
        //CaptureDataText.color = gr.Evaluate(((float)index + 1) / 15f);
    }

    public void StartMagnetometer()
    {
        if (index >= MAX_DATA)
        {
            index = 0;
            CaptureDataText.text = "Attention, donnees ecrasees";
        }
        else
        {
            if (!capturingData)
            {
                GetDataAtPosition(index);
                //CaptureDataText.color = gr.Evaluate(((float) index + 1) / 15f);
                CaptureDataText.text = "Acquisition de donnees (" + GetNumberOfElements().ToString() + "/" + MAX_DATA.ToString() + ")";
                index++;
                capturingData = true;
                StartCoroutine(DelayBeforeCapture(timeBeforeNextCapture));
            }
        }
    }

    #region Index

    public static int GetIndex()
    {
        return index;
    }

    public static int GetNumberOfElements()
    {
        return index + 1;
    }

    public static void ResetIndex()
    {
        index = 0;
    }

    #endregion

    IEnumerator DelayBeforeCapture(float time)
    {
        float elapsedTime = 0f;
        cooldownBar.SetActive(true);

        while (elapsedTime <= time)
        {
            slider.value = elapsedTime / time;
            elapsedTime += Time.deltaTime;
            satelliteStats.battery -= Time.deltaTime * batteryDechargeRate;
            satelliteStats.payloadTemperature += Time.deltaTime * temperatureRate;
            yield return null;
        }

        capturingData = false;
        cooldownBar.SetActive(false);
    }

    #region GetData

    void GetDataAtPosition(int index)
    {
        GetPosition(index);
        //GetTime(index);
        GetMagneticField(index);
    }

    void GetPosition(int index)
    {
        Vector3 position;
        position = magnetometer.localPosition;

        data[index].x = position.x;
        data[index].y = position.y;
        data[index].z = position.z;
    }

    /*void GetTime(int index)
    {
        elapsedTime = Time.realtimeSinceStartup - elapsedTime;
        data[index].timestamp = elapsedTime;

        Debug.Log("Temps ecoule debut sim:" + Time.realtimeSinceStartup);
        Debug.Log("Temps ecoule:" + elapsedTime);
    }*/

    void GetMagneticField(int index)
    {
        data[index].magneticField = MagneticFieldDistribution(data[index].z);
    }

    public float MagneticFieldDistribution(float value)
    {
        if (value < -20f)
        {
            return UnityEngine.Random.Range(0.45f, 0.60f);
        }
        else if (value >= -20f && value < -15f)
        {
            return UnityEngine.Random.Range(0.35f, 0.45f);
        }
        else if (value >= -15f && value < -5f)
        {
            return UnityEngine.Random.Range(0.30f, 0.35f);
        }
        else if (value >= -5f && value < 5f)
        {
            return UnityEngine.Random.Range(0.25f, 0.30f);
        }
        else if (value >= 5f && value < 15f)
        {
            return UnityEngine.Random.Range(0.30f, 0.35f);
        }
        else if (value >= 15f && value < 20f)
        {
            return UnityEngine.Random.Range(0.35f, 0.45f);
        }
        else
        {
            return UnityEngine.Random.Range(0.45f, 0.60f);
        }
    }

    #endregion
}
