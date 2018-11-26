using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {

    public bool collisionWithEarth = false;

    private void Start()
    {
        collisionWithEarth = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionWithEarth = true;
    }
}
