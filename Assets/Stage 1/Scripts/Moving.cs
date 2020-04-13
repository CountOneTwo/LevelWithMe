using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Moving : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private float movementSpeed;

    private CharacterController charController;


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
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

        Vector3 rightMovement = transform.right * horizInput;
        Vector3 forwardMovement = transform.forward * vertInput;


        charController.SimpleMove(forwardMovement + rightMovement);


    }
    }

   /* public class Moving: MonoBehaviour
    {
        [SerializeField] private string horizontalInputName;
        [SerializeField] private string verticalInputName;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float sprintIncrease;
        [SerializeField] private float crouchDecrease;
        [SerializeField] private float slopeForce;
        [SerializeField] private float slopeForceRayLength;
        [SerializeField] private float gravity;
        [SerializeField] private float backwardsFactor;
        [SerializeField] private float airControlFactor;
        [SerializeField] private float gravityScaler;


        private CharacterController charController;

        [SerializeField] private AnimationCurve jumpFallOff;
        [SerializeField] private float jumpMultiplier;
        [SerializeField] private KeyCode jumpKey;
        [SerializeField] private KeyCode sprintKey;
        [SerializeField] private KeyCode crouchKey;

        private float previousY = 0;
        private Vector3 preJumpVector;
        private Vector3 gravityVector;
        private bool isJumping;
        private float adjustedMovementSpeed;

        private void Awake()
        {

            gravityVector = new Vector3(0, -gravity, 0);
            charController = GetComponent<CharacterController>();
            charController.Move(Vector3.zero);
        }

        private void FixedUpdate()
        {
            PlayerMovement();

        }

        private bool OnSlope()
        {
            if (isJumping)
            {
                return false;
            }

            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            {
                if (hit.normal != Vector3.up)
                {
                    return true;
                }
            }
            return false;
        }


        private void PlayerMovement()
        {
            adjustedMovementSpeed = movementSpeed;

            //Sprint();
            //Crouching();

            float horizInput = Input.GetAxis(horizontalInputName);
            float vertInput = Input.GetAxis(verticalInputName);

            if (vertInput < 0)
            {
                vertInput *= backwardsFactor;
            }

            Vector3 forwardMovement = transform.forward * vertInput;
            Vector3 rightMovement = transform.right * horizInput;

            if (charController.isGrounded)
            {
                preJumpVector = forwardMovement + rightMovement;
                charController.Move((Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * adjustedMovementSpeed + gravityVector) * Time.fixedDeltaTime);
            }
            else
            {
                preJumpVector += airControlFactor * (forwardMovement + rightMovement) * Time.fixedDeltaTime;

                if (previousY < transform.position.y)
                {
                    gravityVector.y = -gravity;
                    charController.Move((Vector3.ClampMagnitude(preJumpVector, 1.0f) * adjustedMovementSpeed + gravityVector) * Time.fixedDeltaTime);
                }
                else
                {
                    gravityVector.y -= gravityScaler * Time.fixedDeltaTime;
                    charController.Move((Vector3.ClampMagnitude(preJumpVector, 1.0f) * adjustedMovementSpeed + gravityVector) * Time.fixedDeltaTime);
                }

                previousY = transform.position.y;
            }






            if ((vertInput != 0 || horizInput != 0) && OnSlope())
            {
                charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.fixedDeltaTime);
            }

            JumpInput();

        }

        private void Sprint()
        {
            if (Input.GetKey(sprintKey))
            {
                adjustedMovementSpeed += sprintIncrease;
            }
        }

        private void Crouching()
        {
            if (Input.GetKey(crouchKey))
            {
                charController.height = 1;
            }
            else
            {
                charController.height = 2;
            }


            if (Input.GetKey(crouchKey))
            {
                charController.height = 1;
                adjustedMovementSpeed -= crouchDecrease;
            }

        }

        private void JumpInput()
        {
            if (Input.GetKeyDown(jumpKey) && !isJumping)
            {
                isJumping = true;
                StartCoroutine(JumpEvent());
            }
            /* else if (isJumping)
             {
                 float horizInput = Input.GetAxis(horizontalInputName);
                 float vertInput = Input.GetAxis(verticalInputName);
                 Vector3 forwardMovement = transform.forward * vertInput;
                 Vector3 rightMovement = transform.right * horizInput;
                 charController.Move(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * adjustedMovementSpeed);
             }
        }

        private IEnumerator JumpEvent()
        {
            charController.slopeLimit = 90.0f;
            float timeInAir = 0.0f;

            do
            {



                float jumpForce = jumpFallOff.Evaluate(timeInAir);
                charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.fixedDeltaTime);
                timeInAir += Time.deltaTime;
                yield return null;
            } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

            charController.slopeLimit = 45.0f;
            isJumping = false;
        }

    }
*/

