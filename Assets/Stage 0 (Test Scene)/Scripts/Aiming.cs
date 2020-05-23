using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aiming : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;

    private float xAxisClamp;

    [SerializeField] private Slider mouseSensSlider;
    [SerializeField] private InputField mouseSensInputField;

    private void Awake()
    {
        mouseSensSlider.value = mouseSensitivity;
        mouseSensInputField.text = mouseSensitivity.ToString();

        Cursor.lockState = CursorLockMode.Locked;

        xAxisClamp = 0.0f;
    }

    private void Update()
    {
        if (!HealthAndRespawn_Stage2.dead)
        {
            CameraRotation();
        }
        
    }

    public void ChangeSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
        mouseSensSlider.value = mouseSensitivity;
        mouseSensInputField.text = mouseSensitivity.ToString();
        mouseSensInputField.textComponent.text = mouseSensitivity.ToString();
    }

    public void ChangeSensitivityBySlider()
    {
        ChangeSensitivity(mouseSensSlider.value);
    }

    public void ChangeSensitivityByInputField()
    {
        ChangeSensitivity(float.Parse(mouseSensInputField.text));
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
