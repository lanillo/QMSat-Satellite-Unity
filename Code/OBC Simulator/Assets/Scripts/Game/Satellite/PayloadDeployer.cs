using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Deploys the payload
 * Contains the risk of deployement
 * Increases the payload temperature and reduces battery */
public class PayloadDeployer : MonoBehaviour {

    [Header("Payload Tweaks")]
    public float riskPayloadDeployement = 0.1f;
    public float temperatureRate = 1f;
    public float temperatureTotalCost = 10f;

    [Header("Unity Gameobjects")]
    public GameObject payload;
    public ParticleSystem explosion;
    private GameObject diamond;
    private CollisionDetector collisionDetector;

    [Header("Payload Status")]
    public bool deployPayload = false;
    public bool payloadBroke = false;
    private bool failure = false;
    private bool deployed = false;
    private float timer = 0f;
    private float cumulativeTemperatureDeployment = 0f;
    private bool endParticule = false;
    private bool endGame = false;

    /* For Script accessing */
    private SatelliteStats satelliteStats;

    private void Start()
    {
        deployPayload = false;
        payloadBroke = false;

        // Get payload GameObject and the CollisionDetector
        diamond = payload.transform.GetChild(0).gameObject;
        collisionDetector = diamond.GetComponent<CollisionDetector>();

        satelliteStats = GetComponent<SatelliteStats>();

        if (riskPayloadDeployement >= Random.Range(0f, 1f))
        {
            failure = true;
            endParticule = false;
        }        
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Check if payload is out or particule is done
        if (endParticule || endGame)
        {
            timer += Time.deltaTime;

            if (timer > 3f)
                payloadBroke = true;

            return;
        }

        // Instantiate particule effect when collision with earth happens
        if (collisionDetector.collisionWithEarth && !endParticule)
        {
            Instantiate(explosion, diamond.transform);
            endParticule = true;
        }

        // Deploy Payload After Request
        if (deployPayload)
        {
            timer += Time.deltaTime;

            Vector3 from = payload.transform.localPosition;
            Vector3 to = new Vector3(0, -1.5f, 0f);
            payload.transform.localPosition = Vector3.Lerp(from, to, Time.deltaTime);
        }

        // Once the payload is deployed, let go if risk is true
        if (timer > 1f && failure)
        {
            diamond.transform.parent = null;
            endGame = true;
        }

        // Accumulate Temperature while satellite is beign deployed
        if (deployPayload && !deployed)
        {
            cumulativeTemperatureDeployment += temperatureRate * Time.deltaTime;
            satelliteStats.payloadTemperature += temperatureRate * Time.deltaTime;

            if (cumulativeTemperatureDeployment >= temperatureTotalCost)
                deployed = true;
        }
    }

    public void DeployPayload()
    {
        deployPayload = true;
    }
}
