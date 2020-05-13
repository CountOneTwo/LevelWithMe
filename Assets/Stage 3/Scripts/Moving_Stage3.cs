using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Moving_Stage3 : MonoBehaviour
{

    [Header("Inputs")]
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private string dashButton;

    [Header("Slopes")]
    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

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
    public GameObject dashIcon;
    public Image dashFill;
    public GameObject dashGlow;
    public GameObject speedLines;
    public float dashDuration;
    //dashCooldown is above
    public float minimumSpeed;
    public float maximumSpeed;
    public AnimationCurve speedOverTime;
    [Range(0,1)]
    public float minimumDashControl;
    [Range(0, 1)]
    public float maximumDashControl;
    public AnimationCurve dashControlOverTime;
    [Range(60,179)]
    public float maximumFOV;
    public AnimationCurve fovOverTime;
    public float sliderDeactivationTime;

    bool dashing;
    Vector3 dashDirection;
    float dashProgression;

    Vector3 downwardsVelocity;

    [HideInInspector]
    public bool isGrounded;

    float cooldownTimer;
    [HideInInspector]
    public bool cooldown;
    //bool currentlyJumping;

    private CharacterController charController;

    Vector3 positionLastFrame;
    Vector3 movementLastFrame;
    [HideInInspector]
    public bool activelyMoving;
    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!Regenaration_Stage3.regenerating && !HealthAndRespawn_Stage3.dead )
        {
            if (!dashing)
            {
                PlayerMovement();
            }
            else
            {
                Dash();
            }
           
        }
        
    }

    private void Dash()
    {
        dashProgression += Time.deltaTime;
        if (dashProgression > dashDuration)
        {
            GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView = 60;
            dashing = false;
            PlayerMovement();
            speedLines.SetActive(false);
            return;
        }

        DashCooldown();

        float horizInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);

        Vector3 rightMovement = transform.right * horizInput;
        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 newMovement = Vector3.ClampMagnitude(forwardMovement + rightMovement,1);

        float currentFOV = 60 + ((maximumFOV - 60) * fovOverTime.Evaluate(dashProgression / dashDuration));
        GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView = currentFOV;

        float currentAirControl = minimumDashControl + ((maximumDashControl - minimumDashControl) * dashControlOverTime.Evaluate(dashProgression / dashDuration));
        dashDirection = dashDirection + newMovement * currentAirControl;

        float currentSpeed = minimumSpeed + ((maximumSpeed - minimumSpeed) * speedOverTime.Evaluate(dashProgression/dashDuration));
        charController.Move(Vector3.ClampMagnitude(dashDirection, 1) * currentSpeed * Time.deltaTime);

    }

    void DashCooldown()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldown)
        {
            //  print(1);
            
            dashIcon.SetActive(true);
            dashFill.fillAmount = cooldownTimer / dashCooldown;

            if (cooldownTimer < 0)
            {
                cooldown = false;
                dashGlow.SetActive(true);
            }
        }
        else
        {
            if (cooldownTimer + sliderDeactivationTime < 0)
            {
                dashGlow.SetActive(false);
                dashIcon.SetActive(false);
            }
            
        }
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
                // print("Stop");
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


                activelyMoving = false;
            }
            else
            {
                resultingMovement = ((forwardMovement + rightMovement) * acceleration + movementLastFrame * slideFactor);
                activelyMoving = true;
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            resultingMovement = Vector3.zero;
        }


        if (resultingMovement.magnitude > maxSpeed)
        {
            resultingMovement = Vector3.ClampMagnitude(resultingMovement, maxSpeed);

        }

        movementLastFrame = resultingMovement;

        //----------------------------------------------------------------------------------------------------------//

        //Dash

        if (Input.GetButtonDown(dashButton))
        {
            if (!cooldown)
            {
                if (vertInput == 0 && horizInput == 0)
                {
                    dashDirection = transform.forward;
                }
                else {
                    dashDirection = forwardMovement + rightMovement;
                }
                dashing = true;
                cooldown = true;
                cooldownTimer = dashCooldown;
                dashProgression = 0;
                speedLines.SetActive(true);
                Dash();
                return;
            }
        }

        DashCooldown();



        charController.Move(resultingMovement * Time.deltaTime);



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

        //Adjust y position
        charController.Move(downwardsVelocity * Time.deltaTime);


        //Slopeforce
        if ((vertInput != 0 || horizInput != 0) && OnSlope())
        {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Increase Force"))
        {
            slopeForce = 4;
            slopeForceRayLength = 2;
        }

        if (collider.gameObject.name.Equals("Decrease Force"))
        {
            slopeForce = 1;
            slopeForceRayLength = 1.3f;
        }

        /* if (collider.gameObject.tag == "OutOfBounds")
         {
             Respawn();
         }*/
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
