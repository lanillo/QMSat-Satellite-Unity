using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntennaDeployer : MonoBehaviour {

    private GameObject Antenna;
    private GameObject Antenna1;
    private GameObject Antenna2;
    private GameObject Antenna3;

    public static bool deployAntenna = false;
    public bool scaleAntenna = false;

    private void Start()
    {
        deployAntenna = false;
        Antenna = GameObject.Find("Antenna");
        Antenna1 = GameObject.Find("Antenna1");
        Antenna2 = GameObject.Find("Antenna2");
        Antenna3 = GameObject.Find("Antenna3");
    }

    private void Update()
    {
        if (deployAntenna)
        {
            Vector3 to = new Vector3(0, 90f, 60f);
            Antenna.transform.eulerAngles = Vector3.Lerp(Antenna.transform.rotation.eulerAngles, to, Time.deltaTime);
            Antenna1.transform.eulerAngles = Vector3.Lerp(Antenna1.transform.rotation.eulerAngles, to, Time.deltaTime);
            Antenna2.transform.eulerAngles = Vector3.Lerp(Antenna2.transform.rotation.eulerAngles, to, Time.deltaTime);
            Antenna3.transform.eulerAngles = Vector3.Lerp(Antenna3.transform.rotation.eulerAngles, to, Time.deltaTime);

            if (!scaleAntenna && Antenna.transform.eulerAngles.y > 89.0f)
            {
                StartCoroutine(ScaleOverTime(1f));
                scaleAntenna = true;
            }
        }
    }

    IEnumerator ScaleOverTime(float time)
    {
        Vector3 originalScale_03 = Antenna.transform.localScale;
        Vector3 originalScale_12 = Antenna1.transform.localScale;
        Vector3 destinationScale_03 = new Vector3(4.9f, 0f, 0.1f);
        Vector3 destinationScale_12 = new Vector3(0.1f, 0f, 4.9f);

        float currentTime = 0.0f;

        do
        {
            Antenna.transform.localScale = Vector3.Lerp(originalScale_03, destinationScale_03, currentTime / time);
            Antenna3.transform.localScale = Vector3.Lerp(originalScale_03, destinationScale_03, currentTime / time);
            Antenna1.transform.localScale = Vector3.Lerp(originalScale_12, destinationScale_12, currentTime / time);
            Antenna2.transform.localScale = Vector3.Lerp(originalScale_12, destinationScale_12, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }

    public void DeployAntenna()
    {
        deployAntenna = true;
    }

}
