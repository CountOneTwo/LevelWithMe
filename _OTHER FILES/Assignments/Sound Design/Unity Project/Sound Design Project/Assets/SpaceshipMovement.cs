using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipMovement : MonoBehaviour
{
    public float startingFuel;
    public float startSpeed;
    public float maxSpeed;
    public float acceleration;
    public float deacceleration;
    public float allowedSpeedUponImpact;
    public float rotationalSpeed;
    public float fuelDrainagePerSecond;
    float currentSpeed;
    float currentFuel;
    Vector3 startingPosition;
    Vector3 startingRotation;

    public Text speedText;
    public Text fuelText;



    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.eulerAngles;



        currentFuel = startingFuel;
        currentSpeed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        speedText.text = "Speed: " + currentSpeed;
        fuelText.text = "Fuel: " + currentFuel;


    }

    void Movement()
    {

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rotationalSpeed , 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rotationalSpeed, 0);
        }




        if (Input.GetKey(KeyCode.Space))
        {
            currentFuel -= fuelDrainagePerSecond * Time.deltaTime;
            currentSpeed -= deacceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed += acceleration * Time.deltaTime;

            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
        }



        
        
        
        
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    void Restart()
    {
        transform.position = startingPosition;
        transform.eulerAngles = startingRotation;



        currentFuel = startingFuel;
        currentSpeed = startSpeed;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (currentSpeed > allowedSpeedUponImpact)
        {
            //Explode and Restart
            Restart();


        }
        else
        {

        }
           


    }
}
