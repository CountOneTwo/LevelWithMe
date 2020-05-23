using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aiming_Stage3 : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private Transform playerBody;

    private float xAxisClamp;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        xAxisClamp = 0.0f;
    }

    private void Update()
    {
        if (!Regenaration_Stage3.regenerating && !HealthAndRespawn_Stage3.dead)
        {
            CameraRotation();
        }

    }

    public float GetMouseSens()
    {
        return mouseSensitivity;
    }

    public void SetMouseSens(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
    }

    private void CameraRotation()
    {
        //Get mouse inputs and calibrate them
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Check if camera would flip to the other side leading to an upside down image and preventing that by locking the rotation to the maximum rotation before flipping
        xAxisClamp += mouseY;

        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        //Rotate camera for change in y axis and turn the whole player body for change in x axis
        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    //Necessary to lock rotation while also prevent that it would get locked to early if flicking fast
    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
