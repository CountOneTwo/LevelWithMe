using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Moving_Stage2 : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private string dashButton;
    [SerializeField] private float acceleration;

    public float jumpHeight;
    public float gravity;
    public float additionalFallGravity;
    public float dashSpeed;
    public float slideFactor;
    public float maxSpeed;
    public float dashCooldown;
    Vector3 downwardsVelocity;

    bool isGrounded;

    float cooldownTimer;
    bool cooldown;

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



        //Vector3 resultingMovement = ((forwardMovement + rightMovement) * acceleration  + movementLastFrame * Time.deltaTime);
        Vector3 resultingMovement = ((forwardMovement + rightMovement) * acceleration);






        if (resultingMovement.magnitude > maxSpeed)
        {
            resultingMovement = Vector3.ClampMagnitude(resultingMovement,maxSpeed);
            
        }
        else if (resultingMovement.magnitude < 1)
        {
            resultingMovement = Vector3.zero;
        }

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

            if (cooldownTimer > dashCooldown)
            {
                cooldown = false;
              //  print("Cooldown over");
            }
        }


        charController.Move(resultingMovement * Time.deltaTime /*movementLastFrame * Time.deltaTime*/);
            //movementLastFrame = ((forwardMovement + rightMovement) * acceleration + movementLastFrame) * slideFactor;






          //  movementLastFrame = (resultingMovement  * slideFactor);




        //Gravity
        downwardsVelocity.y += gravity * Time.deltaTime;

        if (transform.position.y < positionLastFrame.y)
        {
            downwardsVelocity.y += additionalFallGravity * Time.deltaTime;
        }

        positionLastFrame = transform.position;


        //Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isGrounded = false;
            downwardsVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        charController.Move(downwardsVelocity * Time.deltaTime);





       
    }

}
