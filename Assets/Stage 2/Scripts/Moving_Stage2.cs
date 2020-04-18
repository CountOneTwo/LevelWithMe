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
    Vector3 velocity;

    bool isGrounded;

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
        if (charController.isGrounded)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }



        float horizInput = Input.GetAxis(horizontalInputName) * acceleration;
        float vertInput = Input.GetAxis(verticalInputName) * acceleration;

        Vector3 rightMovement = transform.right * horizInput;
        Vector3 forwardMovement = transform.forward * vertInput;

        Vector3 resultingMovement = ((forwardMovement + rightMovement) * acceleration  + movementLastFrame * Time.deltaTime);

        
        if (resultingMovement.magnitude > maxSpeed)
        {
            resultingMovement = Vector3.ClampMagnitude(resultingMovement,maxSpeed);
            
        }
        else if (resultingMovement.magnitude < 1)
        {
            resultingMovement = Vector3.zero;
        }

        print(resultingMovement.magnitude);

        if (Input.GetButtonDown(dashButton))
        {
            charController.Move((forwardMovement + rightMovement) * Time.deltaTime * dashSpeed);
        }
        else
        {
            

            charController.Move(resultingMovement * Time.deltaTime);
            //movementLastFrame = ((forwardMovement + rightMovement) * acceleration + movementLastFrame) * slideFactor;






            movementLastFrame = (resultingMovement  * slideFactor);
        }

       



        velocity.y += gravity * Time.deltaTime;

        if (transform.position.y < positionLastFrame.y)
        {
            velocity.y += additionalFallGravity * Time.deltaTime;
        }

        positionLastFrame = transform.position;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isGrounded = false;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        charController.Move(velocity * Time.deltaTime);





       
    }

}
