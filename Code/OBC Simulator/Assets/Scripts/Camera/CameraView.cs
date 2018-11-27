using UnityEngine;
using System.Collections;

public class CameraView : MonoBehaviour
{
    [Header("Mouse Sensibility")]
    public float mouseSensitivityX = 5.0f;
    public float mouseSensitivityY = 5.0f;
    public float smoothSpeed = 0.125f;

    [Header("Unity Objects")]
    public Transform cubesat;
    public Transform earth;

    private float rotY = 0.0f;
    private bool autoLook = true;

    void Start()
    {
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;

        autoLook = true;
    }

    void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            if (autoLook)
                autoLook = false;
            else
                autoLook = true;
        }

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
        // Orbit set by QMSat.CameraOrbit
        Vector3 desiredPosition = cubesat.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        if (autoLook)
            transform.LookAt(earth);

    }
}
