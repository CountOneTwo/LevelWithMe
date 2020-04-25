using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aiming : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private Transform playerBody;

    private float xAxisClamp;

    public Slider mouseSensSlider;
    public InputField mouseSensInputField;

    private void Awake()
    {
        mouseSensSlider.value = mouseSensitivity;
        mouseSensInputField.text = mouseSensitivity.ToString();
        Cursor.lockState = CursorLockMode.Locked;
        xAxisClamp = 0.0f;
    }

    private void Update()
    {
        CameraRotation();
    }

    public void ChangeSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
        mouseSensSlider.value = mouseSensitivity;
        Debug.Log(mouseSensitivity);
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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

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

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
