using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObstacle : MonoBehaviour {

    public GameObject groundStation;
    public GameObject cubesat;
    public GameObject sunlight;

    public bool commCollision = false;
    public bool lightCollision = false;

    private void Start()
    {     
        commCollision = CanTransmit();
        lightCollision = SolarCharging();

        PlayerPrefsX.SetBool("commCollision", commCollision);
        PlayerPrefsX.SetBool("lightCollision", lightCollision);

    }

    private void Update()
    {
        commCollision = CanTransmit();
        lightCollision = SolarCharging();

        PlayerPrefsX.SetBool("commCollision", commCollision);
        PlayerPrefsX.SetBool("lightCollision", lightCollision);
    }

    bool CanTransmit()
    {
        bool hitStation = false;

        RaycastHit hit;
        Vector3 direction = groundStation.transform.position - cubesat.transform.position;
        Debug.DrawLine(groundStation.transform.position, cubesat.transform.position);

        if (Physics.Raycast(cubesat.transform.position, direction, out hit)) //Range if we want
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

        return hitStation;
    }

    bool SolarCharging()
    {
        bool hitSun = false;

        RaycastHit hit;
        Vector3 direction = sunlight.transform.position - cubesat.transform.position;
        Debug.DrawLine(sunlight.transform.position, cubesat.transform.position);
        if (Physics.Raycast(cubesat.transform.position, direction, out hit)) //Range if we want
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

        return hitSun;
    }

}
