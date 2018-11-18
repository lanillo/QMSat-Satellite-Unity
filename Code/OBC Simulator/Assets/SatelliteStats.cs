using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteStats : MonoBehaviour {

    public float battery = 100f;
    public bool commCollision = false;
    public bool lightCollision = false;

    private void Start()
    {
        commCollision = PlayerPrefsX.GetBool("commCollision");
        lightCollision = PlayerPrefsX.GetBool("lightCollision");
    }

    private void Update()
    {
        commCollision = PlayerPrefsX.GetBool("commCollision");
        lightCollision = PlayerPrefsX.GetBool("lightCollision");
    }

}
