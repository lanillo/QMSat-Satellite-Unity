using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransmitData : MonoBehaviour {

    public GameObject cooldownBar;
    private Slider slider;

    public static bool transmittingData = false;
    public float timePerData = 0.5f;
    private float timeBeforeNextTransmit = 0f;
    private int dataAmount = 0;

    private void Start()
    {
        List<MagnetometerData> dataset = new List<MagnetometerData>();
        cooldownBar.SetActive(false);
        slider = cooldownBar.GetComponentInChildren<Slider>();

        Debug.Log(slider);
    }

    public void TransmitDataToGroundStation()
    {
        dataAmount = CaptureData.GetIndex();
        Debug.Log("Data amount: " + dataAmount);
        timeBeforeNextTransmit = timePerData * (float)dataAmount;
        StartCoroutine(DelayBeforeTransmit(timeBeforeNextTransmit));
    }

    IEnumerator DelayBeforeTransmit(float time)
    {

        Debug.Log("Start of communication");
        float elapsedTime = 0f;
        cooldownBar.SetActive(true);

        while (elapsedTime <= timeBeforeNextTransmit)
        {
            slider.value = elapsedTime / timeBeforeNextTransmit;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transmittingData = false;
        cooldownBar.SetActive(false);
    }
}
