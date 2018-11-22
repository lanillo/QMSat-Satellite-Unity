using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaptureData : MonoBehaviour {

    public MagnetometerData[] data;
    public Transform magnetometer;

    private int index = 0;
    private const int MAX_DATA = 15;

    private void Start()
    {
        data = new MagnetometerData[MAX_DATA];
        index = 0;
    }

    public void StartMagnetometer()
    {
        if (index >= MAX_DATA)
        {
            index = 0;
            Debug.Log("Warning, overwritting data");
        }
        else
        {
            GetDataAtPosition(index);
            index++;
        }
    }

    void GetDataAtPosition(int index)
    {
        GetPosition(index);
        GetTime(index);
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

    void GetTime(int index)
    {
        data[index].timestamp = DateTime.Now;
    }

    void GetMagneticField(int index)
    {
        data[index].magneticField = MagneticFieldDistribution(data[index].z);
    }

    public float MagneticFieldDistribution(float value)
    {
        if (value < -20f)
        {
            return UnityEngine.Random.Range(0.25f, 0.30f);
        }
        else if (value >= -20f && value < -15f)
        {
            return UnityEngine.Random.Range(0.30f, 0.35f);
        }
        else if (value >= -15f && value < -5f)
        {
            return UnityEngine.Random.Range(0.35f, 0.45f);
        }
        else if (value >= -5f && value < 5f)
        {
            return UnityEngine.Random.Range(0.45f, 0.60f);
        }
        else if (value >= 5f && value < 15f)
        {
            return UnityEngine.Random.Range(0.35f, 0.45f);
        }
        else if (value >= 15f && value < 20f)
        {
            return UnityEngine.Random.Range(0.30f, 0.35f);
        }
        else
        {
            return UnityEngine.Random.Range(0.25f, 0.30f);
        }
    }
}
