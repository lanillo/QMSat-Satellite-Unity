﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObstacle : MonoBehaviour {

    public GameObject groundStation;
    public GameObject cubesat;
    public GameObject sunlight;

    public bool commCollision = false;
    public bool lightCollision = false;

    private LineRenderer line;

    private void Start()
    {
        /*// Add a Line Renderer to the GameObject
        line = this.gameObject.AddComponent<LineRenderer>();
        // Set the width of the Line Renderer
        line.startWidth = 0.1f;
        // Set the number of vertex fo the Line Renderer
        line.positionCount = 2;*/
     
        commCollision = CanTransmit();
        lightCollision = SolarCharging();

        PlayerPrefsX.SetBool("commCollision", commCollision);
        PlayerPrefsX.SetBool("lightCollision", lightCollision);

    }

    private void Update()
    {
        /*// Check if the GameObjects are not null
        if (target != null && cubesat != null)
        {
            // Update position of the two vertex of the Line Renderer
            line.SetPosition(0, target.transform.position);
            line.SetPosition(1, cubesat.transform.position);
        }*/

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