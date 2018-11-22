using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteStats : MonoBehaviour {

    [Header("Abilities of Satellite")]
    public bool commCollision = false;
    public bool lightCollision = false;
    public bool canTransmit = false;

    public static float battery = 100f;
    [Header("Battery Options")]
    public float batteryDechargeRate = 0.1f;
    public float batteryRechargeRate = 0.1f;
    [Space(10)]
    public float antennaDeployementRate = 1f;
    public float antennaDeployementTotalCost = 10f;
    private float cumulativeAntennaDeployment = 0f;
    [Space(10)]
    public float batteryCaptureDataDechargeRate = 0.5f;
    [Space(10)]



    private bool antennaDeployed = false;

    [Header("Unity Text Fields")]
    public Text batteryText;

    [Header("Unity Objects")]
    public Gradient gr;
    public GameObject comm;
    private Button commButton;

    //public bool test = false;

    private void Start()
    {
        battery = 100f;
        commCollision = PlayerPrefsX.GetBool("commCollision");
        lightCollision = PlayerPrefsX.GetBool("lightCollision");
        commButton = comm.GetComponent<Button>();

        batteryText.supportRichText = true;
    }

    private void Update()
    {
        if (GameManager.gameOver)
            return;

        commCollision = PlayerPrefsX.GetBool("commCollision");
        lightCollision = PlayerPrefsX.GetBool("lightCollision");

        BatteryUpdate();
    }

    void BatteryUpdate()
    {
        // Check if antenna is being deployed
        if (AntennaDeployer.deployAntenna && !antennaDeployed)
        {
            cumulativeAntennaDeployment += antennaDeployementRate * Time.deltaTime;
            battery -= antennaDeployementRate * Time.deltaTime;

            if (cumulativeAntennaDeployment >= antennaDeployementTotalCost)
            {
                antennaDeployed = true;
            }
        }

        // If data is being captured
        if (CaptureData.capturingData)
        {
            battery -= batteryCaptureDataDechargeRate * Time.deltaTime;
        }

        // Check if you can transmit data to ground station (enable or disabl button
        CanTransmit();

        // Check if it's on light
        if (!lightCollision)
        {
            battery -= batteryDechargeRate * Time.deltaTime;
        }
        else
        {
            battery += batteryRechargeRate * Time.deltaTime;
        }

        battery = Mathf.Clamp(battery, 0.00f, 100.0f);

        batteryText.color = gr.Evaluate(battery / 100f);
        batteryText.text = "Batteries: " + battery.ToString("0.00") + "%";

    }

    void CanTransmit()
    {
        // If antenna is not deployed, cannot trasnmit
        if (!antennaDeployed)
        {
            commButton.interactable = false;
            canTransmit = false;
        } //Check if you want them to communicate or not (hard mode ??)
        else if (commCollision)
        {
            commButton.interactable = true;
            canTransmit = true;
        }
        else
        {
            commButton.interactable = false;
            canTransmit = false;
        }
    }
}
