using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class containing 1 data acquired by the magnetometer */
[System.Serializable]
public struct MagnetometerData
{
    public float x;
    public float y;
    public float z;

    public float magneticField;

    //public float timestamp;


}
