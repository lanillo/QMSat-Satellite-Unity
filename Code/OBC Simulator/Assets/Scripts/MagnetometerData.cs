using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct MagnetometerData
{
    public float x;
    public float y;
    public float z;

    public float magneticField;

    public DateTime timestamp;
}
