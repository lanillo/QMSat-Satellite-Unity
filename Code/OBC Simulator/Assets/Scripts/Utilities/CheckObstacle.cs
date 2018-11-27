using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObstacle : MonoBehaviour {

    [Header("Unity GameManager")]
    public GameObject gameManager;
    private SatelliteStats satelliteStats;

    [Header("Unity GameObjects to Raycast")]
    public GameObject cubesat;
    [Space(10)]
    public GameObject groundStation;
    public GameObject sunlight;

    private bool[] temporaryValues;
    private int index;

    private void Start()
    {
        // Get GameManager
        satelliteStats = gameManager.GetComponent<SatelliteStats>();

        // Initialize ind
        temporaryValues = new bool[5];
        InitializeTempValues();

        satelliteStats.commCollision = CanTransmit();
        satelliteStats.lightCollision = SolarCharging();
    }

    private void Update()
    {
        satelliteStats.commCollision = CanTransmit();
        satelliteStats.lightCollision = SolarCharging();
    }

    bool CanTransmit()
    {
        bool hitStation;// = false;

        RaycastHit hit;
        Vector3 direction = groundStation.transform.position - cubesat.transform.position;
        Debug.DrawLine(groundStation.transform.position, cubesat.transform.position);

        if (Physics.Raycast(cubesat.transform.position, direction, out hit))
        {
            switch (hit.transform.gameObject.tag)
            {
                case "CommStation":
                    hitStation = true;
                    break;
                default:
                    hitStation = false;
                    break;
            }
        }
        else
            hitStation = false;

        // Due to communications lag with raycast, we're going to do a debouncing before changing state...
        AddNewValue(hitStation);
        hitStation = CheckLastValues();

        return hitStation;
    }

    bool SolarCharging()
    {
        bool hitSun;// = false;

        RaycastHit hit;
        Vector3 direction = sunlight.transform.position - cubesat.transform.position;
        Debug.DrawLine(sunlight.transform.position, cubesat.transform.position);

        if (Physics.Raycast(cubesat.transform.position, direction, out hit))
        {
            switch (hit.transform.gameObject.tag)
            {
                case "SunLight":
                    hitSun = true;
                    break;
                default:
                    hitSun = false;
                    break;
            }
        }
        else
            hitSun = false;

        return hitSun;
    }

    #region arrayManagement

    public void InitializeTempValues()
    {
        for (int i = 0; i < 5; i++)
        {
            temporaryValues[i] = false; 
        }
    }

    public void AddNewValue(bool value)
    {
        for (int i = 0; i < 4; i++)
        {
            temporaryValues[i] = temporaryValues[i + 1];
        }

        temporaryValues[4] = value;
    }

    public bool CheckLastValues()
    {
        int count = 0;

        if (temporaryValues[4] == true)
            return true;

        for (int i = 0; i < 5; i++)
        {
            if (temporaryValues[i])
                count++;
            else
                count--;
        }

        if (count == 1)
            return true;
        else
            return false;
    }

    #endregion
}
