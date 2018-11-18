using UnityEngine;
using System.Collections;

public class CameraView : MonoBehaviour
{
    public float speedNormal = 10.0f;
    public float speedFast = 50.0f;

    public float mouseSensitivityX = 5.0f;
    public float mouseSensitivityY = 5.0f;

    float rotY = 0.0f;

    public Transform cubesat;
    public Transform earth;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public bool autoLook = true;


    void Start()
    {
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    void Update()
    {
        // rotation        
        if (Input.GetMouseButton(1))
        {
            if (autoLook)
                return;

            float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivityX;
            rotY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            rotY = Mathf.Clamp(rotY, -89.5f, 89.5f);
            transform.localEulerAngles = new Vector3(-rotY, rotX, 0.0f);
        }

    }

    private void LateUpdate()
    {
        // Orbit set by CameraOrbit

        Vector3 desiredPosition = cubesat.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        if (autoLook)
            transform.LookAt(earth);

    }
}
