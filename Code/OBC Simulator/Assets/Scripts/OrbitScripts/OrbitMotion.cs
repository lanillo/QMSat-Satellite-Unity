using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrbitMotion : MonoBehaviour
{
    public Transform orbitingObject;
    public Ellipse orbitPath;
    public Text timeText;

    [Range(0f, 1f)]
    public float orbitProgress = 0f;
    public float orbitPeriod = 3f;
    public bool orbitActive = true;
    public bool timeMonthActive = false;

	// Use this for initialization
	void Start ()
    {
        // Check if there is an orbiting object
        if (orbitingObject == null)
        {
            orbitActive = false;
            return;
        }

        if (timeMonthActive)
            timeText.text = FindMonth() + "  ";

        SetOrbitingObject();
        StartCoroutine(AnimateOrbit());
	}

    private void Update()
    {
        if (timeMonthActive)
        {
            if (!timeText.text.Equals(FindMonth()))
                timeText.text = FindMonth() + "  ";
            else
                return;
        }        
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
            orbitProgress %= 1f;
            SetOrbitingObject();
            yield return null;
        }
    }

    public string FindMonth()
    {
        if (orbitProgress > 11.9f / 12f)
        {
            GameManager.timeOver = true;
            return "Fin";
        }            
        else if (orbitProgress > 11f / 12f)
            return "Decembre";
        else if (orbitProgress > 10f / 12f)
            return "Novembre";
        else if (orbitProgress > 9f / 12f)
            return "Octobre";
        else if (orbitProgress > 8f / 12f)
            return "Septembre";
        else if (orbitProgress > 7f / 12f)
            return "Aout";
        else if (orbitProgress > 6f / 12f)
            return "Juillet";
        else if (orbitProgress > 5f / 12f)
            return "Juin";
        else if (orbitProgress > 4f / 12f)
            return "Mai";
        else if (orbitProgress > 3f / 12f)
            return "Avril";
        else if (orbitProgress > 2f / 12f)
            return "Mars";
        else if (orbitProgress > 1f / 12f)
            return "Fevrier";
        else
            return "Janvier";
    }
}
