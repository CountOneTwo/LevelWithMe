using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Moving_Stage2 : MonoBehaviour
{

    [Header("Inputs")]
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private string dashButton;

    [Header("Slopes")]
    [SerializeField] public float slopeForce;
    [SerializeField] public float slopeForceRayLength;

    [Header("Jump & Gravity")]
    public float jumpHeight;
    public float gravity;
    public float additionalFallGravity;
    public float airControlFactor;

    [Header("Speed & Acceleration")]
    public float slideFactor;
    public float maxSpeed;
    [SerializeField] private float acceleration;
    public float deaccelerationLogarithmBase;
    public bool instantStop;

    [Header("Dash")]
    public float dashSpeed;
    public float dashCooldown;
    public Slider dashSlider;

    Vector3 downwardsVelocity;
 
    bool isGrounded;

    float cooldownTimer;
    bool cooldown;
    //bool currentlyJumping;

    private CharacterController charController;

    Vector3 positionLastFrame;
    Vector3 movementLastFrame;
    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        

        //ResetJump
        if (charController.isGrounded)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        //Gravity
        if (isGrounded && downwardsVelocity.y < 0)
        {
            downwardsVelocity.y = -2f;
        }


        //Get Input
        float horizInput = Input.GetAxis(horizontalInputName) * acceleration;
        float vertInput = Input.GetAxis(verticalInputName) * acceleration;

        Vector3 rightMovement = transform.right * horizInput;
        Vector3 forwardMovement = transform.forward * vertInput;

        Vector3 resultingMovement;
        //Vector3 resultingMovement = ((forwardMovement + rightMovement) * acceleration  + movementLastFrame * Time.deltaTime);
        if (!isGrounded)
        {
            resultingMovement = ((forwardMovement + rightMovement) * acceleration * airControlFactor + movementLastFrame);
        }
        else
        {
            if (vertInput == 0 && horizInput == 0)
            {
                print("Stop");
                if (instantStop)
                {
                    resultingMovement = Vector3.zero;
                }
                else
                {
                    float clampingMagnitude = Mathf.Log(movementLastFrame.magnitude, deaccelerationLogarithmBase);
                    if (Mathf.Abs(clampingMagnitude) < 1)
                    {
                        resultingMovement = Vector3.zero;
                    }
                    else
                    {
                        resultingMovement = ((forwardMovement + rightMovement) * acceleration + Vector3.ClampMagnitude(movementLastFrame, clampingMagnitude));
                    }
                }


               
            }
            else
            {
                resultingMovement = ((forwardMovement + rightMovement) * acceleration + movementLastFrame * slideFactor);
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            resultingMovement = Vector3.zero;
        }


        if (resultingMovement.magnitude > maxSpeed)
        {
            resultingMovement = Vector3.ClampMagnitude(resultingMovement,maxSpeed);
            
        }

        movementLastFrame = resultingMovement;

        print(resultingMovement.magnitude * Time.deltaTime);
     /*   if (resultingMovement.magnitude - slideFactor < 0)
        {
            movementLastFrame = Vector3.ClampMagnitude(resultingMovement, 0);
        }
        else
        {
            movementLastFrame = Vector3.ClampMagnitude(resultingMovement, Mathf.Clamp(resultingMovement.magnitude * resultingMovement.magnitude, 0,maxSpeed) - slideFactor);
        }*/

        


        //print(resultingMovement.magnitude);

        //Dash
        if (Input.GetButtonDown(dashButton))
        {
           // print(2);
            if (!cooldown)
            {
                cooldown = true;
                cooldownTimer = 0;
                charController.Move((forwardMovement + rightMovement) * dashSpeed /*movementLastFrame * Time.deltaTime*/);
             //   print("dash");
                return;
                
            }

        }

        if (cooldown)
        {
          //  print(1);
            cooldownTimer += Time.deltaTime;
            dashSlider.gameObject.SetActive(true);
            dashSlider.value = cooldownTimer / dashCooldown;

            if (cooldownTimer > dashCooldown)
            {
                cooldown = false;
              //  print("Cooldown over");
            }
        }
        else
        {
            dashSlider.gameObject.SetActive(false);


        }


        charController.Move(resultingMovement * Time.deltaTime /*movementLastFrame * Time.deltaTime*/);
            //movementLastFrame = ((forwardMovement + rightMovement) * acceleration + movementLastFrame) * slideFactor;
          //  movementLastFrame = (resultingMovement  * slideFactor);

        //Gravity
        downwardsVelocity.y += gravity * Time.deltaTime;

        if (transform.position.y <= positionLastFrame.y)
        {
            downwardsVelocity.y += additionalFallGravity * Time.deltaTime;
        }

        if (downwardsVelocity.y > 0 && transform.position.y <= positionLastFrame.y)
        {
            downwardsVelocity.y = -2f;
        }

        positionLastFrame = transform.position;




        //Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isGrounded = false;
            downwardsVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        charController.Move(downwardsVelocity * Time.deltaTime);


        if ((vertInput != 0 || horizInput != 0) && OnSlope())
        {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
           // print("yo");
        }

    }

    private bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }


}
