using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class S_Moving_Final : MonoBehaviour
{

    [Header("Inputs")]
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;

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

    Vector3 downwardsVelocity;

    [HideInInspector]
    public bool isGrounded;

    //bool currentlyJumping;

    private CharacterController charController;

    Vector3 positionLastFrame;
    Vector3 movementLastFrame;
    [HideInInspector]
    public bool activelyMoving;

    private bool FirstFootstep;
    private bool IsMoving;

    public float Airtime;
    private bool hasPlayedJumpstart;
    private bool isJumping;


    public float CurrentVolumeLanding;
    private bool GroundedLastFrame;

    private bool isDashing;

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
                IsMoving = false;
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
                        IsMoving = true;
                    }
                }


                activelyMoving = false;
            }
            else
            {
                resultingMovement = ((forwardMovement + rightMovement) * acceleration + movementLastFrame * slideFactor);
                activelyMoving = true;
                IsMoving = true;
            }
        }


        if (resultingMovement.magnitude > maxSpeed)
        {
            resultingMovement = Vector3.ClampMagnitude(resultingMovement, maxSpeed);

        }

        movementLastFrame = resultingMovement;

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
            isJumping = true;
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
