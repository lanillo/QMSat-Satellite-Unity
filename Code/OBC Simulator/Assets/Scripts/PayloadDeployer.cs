using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadDeployer : MonoBehaviour {

    public GameObject payload;
    private GameObject diamond;
    private CollisionDetector cd;

    public static bool deployPayload = false;
    public float riskPayloadDeployement = 1.1f;

    public bool failure = false;
    private float timer = 0f;

    private void Start()
    {
        deployPayload = false;

        if (riskPayloadDeployement >= Random.Range(0f, 1f))
        {
            Debug.Log("Failure with Payload");
            failure = true;
            diamond = payload.transform.GetChild(0).gameObject;
            cd = diamond.GetComponent<CollisionDetector>();
        }        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (cd.collisionWithEarth)
        {
            
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
        }        
    }

    public void DeployPayload()
    {
        deployPayload = true;
    }
}
