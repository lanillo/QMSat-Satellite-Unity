using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Makes object rotate from its center */
public class Rotate : MonoBehaviour {

    [Tooltip("Spin: Yes or No")]
    public bool spin;
    public float speed = 10f;

    public bool clockwise = true;
    [HideInInspector]
    public float direction = 1f;
    [HideInInspector]
    public float directionChangeSpeed = 2f;

    void Update()
    {
        if (direction < 1f)
            direction += Time.deltaTime / (directionChangeSpeed / 2);

        if (spin)
        {
            if (clockwise)
                transform.Rotate(Vector3.up, (speed * direction) * Time.deltaTime);
            else
                transform.Rotate(-Vector3.up, (speed * direction) * Time.deltaTime);
        }
    }
}
