using UnityEngine;

/* Detect collision with objects */
public class CollisionDetector : MonoBehaviour {

    [HideInInspector]
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
