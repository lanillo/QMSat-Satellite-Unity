using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntennaDeployer : MonoBehaviour {

    public ParticleSystem explosion;

    public GameObject antenna;
    public GameObject antenna1;
    public GameObject antenna2;
    public GameObject antenna3;
    private Transform antennaGroup;

    public static bool deployAntenna = false;
    public static bool antennaBroke = false;
    public bool scaleAntenna = false;
    public float riskAntennaDeployement = 0.1f;

    public float temperatureRate = 1f;
    public float temperatureTotalCost = 10f;
    private float cumulativeTemperatureDeployment = 0f;
    private bool deployed = false;

    private bool failure = false;
    private bool endEffect = false;
    private float timer = 0f;

    private void Start()
    {
        deployAntenna = false;
        endEffect = false;
        antennaBroke = false;
        timer = 0f;
        antennaGroup = antenna.gameObject.GetComponentInParent<Transform>();

        if (riskAntennaDeployement >= Random.Range(0f, 1f))
            failure = true;
    }

    private void Update()
    {
        if (deployAntenna && failure)
        {
            if (!endEffect)
                Instantiate(explosion, antennaGroup.transform);
            endEffect = true;

            timer += Time.deltaTime;

            if (timer >= 1f)
                antennaBroke = true;

            return;
        }

        if (deployAntenna)
        {
            Vector3 to = new Vector3(0, 90f, 60f);
            antenna.transform.eulerAngles = Vector3.Lerp(antenna.transform.rotation.eulerAngles, to, Time.deltaTime);
            antenna1.transform.eulerAngles = Vector3.Lerp(antenna1.transform.rotation.eulerAngles, to, Time.deltaTime);
            antenna2.transform.eulerAngles = Vector3.Lerp(antenna2.transform.rotation.eulerAngles, to, Time.deltaTime);
            antenna3.transform.eulerAngles = Vector3.Lerp(antenna3.transform.rotation.eulerAngles, to, Time.deltaTime);

            if (!scaleAntenna && antenna.transform.eulerAngles.y > 89.0f)
            {
                StartCoroutine(ScaleOverTime(1f));
                scaleAntenna = true;
            }
        }

        if (deployAntenna && !deployed)
        {
            cumulativeTemperatureDeployment += temperatureRate * Time.deltaTime;
            SatelliteStats.telecomTemperature += temperatureRate * Time.deltaTime;

            if (cumulativeTemperatureDeployment >= temperatureTotalCost)
            {
                deployed = true;
            }
        }
    }

    IEnumerator ScaleOverTime(float time)
    {
        Vector3 originalScale_03 = antenna.transform.localScale;
        Vector3 originalScale_12 = antenna1.transform.localScale;
        Vector3 destinationScale_03 = new Vector3(4.9f, 0.05f, 0.1f);
        Vector3 destinationScale_12 = new Vector3(0.1f, 0.05f, 4.9f);

        float currentTime = 0.0f;

        do
        {
            antenna.transform.localScale = Vector3.Lerp(originalScale_03, destinationScale_03, currentTime / time);
            antenna3.transform.localScale = Vector3.Lerp(originalScale_03, destinationScale_03, currentTime / time);
            antenna1.transform.localScale = Vector3.Lerp(originalScale_12, destinationScale_12, currentTime / time);
            antenna2.transform.localScale = Vector3.Lerp(originalScale_12, destinationScale_12, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }

    public void DeployAntenna()
    {
        deployAntenna = true;
    }

}
