using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadDeployer : MonoBehaviour {

    public GameObject payload;
    public ParticleSystem explosion;
    private GameObject diamond;
    private CollisionDetector cd;

    public static bool deployPayload = false;
    public float riskPayloadDeployement = 0.1f;

    public float temperatureRate = 1f;
    public float temperatureTotalCost = 10f;
    private float cumulativeTemperatureDeployment = 0f;
    private bool deployed = false;

    private bool failure = false;
    private float timer = 0f;
    private bool endParticule = false;
    private bool endGame = false;
    public static bool payloadBroke = false;

    private void Start()
    {
        deployPayload = false;
        deployed = false;
        endGame = false;
        payloadBroke = false;
        diamond = payload.transform.GetChild(0).gameObject;
        cd = diamond.GetComponent<CollisionDetector>();

        if (riskPayloadDeployement >= Random.Range(0f, 1f))
        {
            failure = true;
            endParticule = false;
        }        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (endParticule || endGame)
        {
            timer += Time.deltaTime;

            if (timer > 3f)
            {
                payloadBroke = true;
            }

            return;
        }

        if (cd.collisionWithEarth && !endParticule)
        {
            Instantiate(explosion, diamond.transform);
            endParticule = true;
        }

        if (deployPayload)
        {
            timer += Time.deltaTime;

            Vector3 from = payload.transform.localPosition;
            Vector3 to = new Vector3(0, -1.5f, 0f);
            payload.transform.localPosition = Vector3.Lerp(from, to, Time.deltaTime);
        }

        if (timer > 1f && failure)
        {
            diamond.transform.parent = null;
            endGame = true;
        }

        if (deployPayload && !deployed)
        {
            cumulativeTemperatureDeployment += temperatureRate * Time.deltaTime;
            SatelliteStats.payloadTemperature += temperatureRate * Time.deltaTime;

            if (cumulativeTemperatureDeployment >= temperatureTotalCost)
            {
                deployed = true;
            }
        }
    }

    public void DeployPayload()
    {
        deployPayload = true;
    }
}
