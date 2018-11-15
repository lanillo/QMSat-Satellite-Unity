using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMotion : MonoBehaviour
{

    public Transform orbitingObject;
    public Ellipse orbitPath;

    [Range(0f, 1f)]
    public float orbitProgress = 0f;
    public float orbitPeriod = 3f;
    public bool orbitActive = true;

	// Use this for initialization
	void Start ()
    {
        // Check if there is an orbiting object
        if (orbitingObject == null)
        {
            orbitActive = false;
            return;
        }

        SetOrbitingObject();
        StartCoroutine(AnimateOrbit());

	}

    void SetOrbitingObject()
    {
        Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);
        orbitingObject.localPosition = new Vector3(orbitPos.x, 1f, orbitPos.y);
    }

    IEnumerator AnimateOrbit()
    {
        if (orbitPeriod < 0.5f)
            orbitPeriod = 0.5f;

        float orbitSpeed = 1f / orbitPeriod;
        while (orbitActive)
        {
            orbitProgress += Time.deltaTime * orbitSpeed;
            orbitProgress %= 1f; // reset to 1;
            SetOrbitingObject();
            yield return null;
        }
    }
}
