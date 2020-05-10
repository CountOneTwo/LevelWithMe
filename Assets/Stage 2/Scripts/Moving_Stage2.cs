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
    public Slider dashSlider;

    [Header("Footsteps")]
    public AudioSource FootstepAudioSource;
    public AudioClip[] FootstepSounds;
    public float FootstepCooldown;
    public bool PitchChangeEnabledFootsteps;
    [Range(-3, 3)] public float CurrentPitchFootsteps;
    [Range(-3, 3)] public float MinPitchFootsteps;
    [Range(-3, 3)] public float MaxPitchFootsteps;
    public bool VolumeChangeEnabledFootsteps;
    [Range(0, 1)] public float CurrentVolumeFootsteps;
    [Range(0, 1)] public float MinVolumeFootsteps;
    [Range(0, 1)] public float MaxVolumeFootsteps;

    private float FootstepCooldownTimer;
    private bool FirstFootstep;
    private bool IsMoving;
    //private AudioClip LastFootstep;

    [Header("Jumpstart Sound")]
    public AudioClip JumpstartSound;
    [Range(0, 2)] public float delayTillPlayJumpstart;
    [Range(0, 1)] public float VolumeJumpstart;
    public bool PitchChangeEnabledJumpstart;
    [Range(-3, 3)] public float CurrentPitchJumpstart;
    [Range(-3, 3)] public float MinPitchJumpstart;
    [Range(-3, 3)] public float MaxPitchJumpstart;

    [Range(0, 3)] private float JumpstartDelayTimer;
    private bool hasPlayedJumpstart;
    private bool isJumping;

    [Header("Landing Sounds")]
    public AudioClip[] LandingSounds;
    public bool PitchChangeEnabledLanding;
    [Range(-3, 3)] public float CurrentPitchLanding;
    [Range(-3, 3)] public float MinPitchLanding;
    [Range(-3, 3)] public float MaxPitchLanding;
    public bool FallAffectsLandingVolume;
    [Range(0, 1)] public float NormalVolumeLanding;
    [Range(0, 1)] public float MaxVolumeLanding;

    public float CurrentVolumeLanding;

    [Header("Dash Sound")]
    public AudioSource DashAudioSource;
    public AudioClip Dashsound;
    [Range(0, 1)] public float DashVolume;

    private bool isDashing;

    Vector3 downwardsVelocity;
 
    bool isGrounded;

    float cooldownTimer;

    [HideInInspector]
    public bool cooldown;
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
        Sounds();
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
                IsMoving = false;
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
                IsMoving = true;
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

      //  print(resultingMovement.magnitude * Time.deltaTime);
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
                isDashing = true;
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
            isJumping = true;
        }

        charController.Move(downwardsVelocity * Time.deltaTime);


        if ((vertInput != 0 || horizInput != 0) && OnSlope())
        {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
           // print("yo");
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

    private void Sounds()
    {
        //Footsteps
        if(isGrounded == true)
        {
            if (IsMoving == true)
            {
                FootstepCooldownTimer += Time.deltaTime;

                if (FirstFootstep == true || FootstepCooldownTimer > FootstepCooldown)
                {
                    if(PitchChangeEnabledFootsteps == true)
                    {
                        CurrentPitchFootsteps = Random.Range(MinPitchFootsteps, MaxPitchFootsteps);
                    }
                    if(VolumeChangeEnabledFootsteps == true)
                    {
                        CurrentVolumeFootsteps = Random.Range(MinVolumeFootsteps, MaxVolumeFootsteps);
                    }
                    FootstepAudioSource.clip = FootstepSounds[Random.Range(0, FootstepSounds.Length)];
                    FootstepAudioSource.pitch = CurrentPitchFootsteps;
                    FootstepAudioSource.volume = CurrentVolumeFootsteps;
                    FootstepAudioSource.Play();
                    FirstFootstep = false;
                    FootstepCooldownTimer = 0;
                }
            }
            else FirstFootstep = true;
        }
        //Jump start + landing
        if (isJumping == true)
        {
            if(isGrounded == false)
            {
                JumpstartDelayTimer += Time.deltaTime;
                if(JumpstartDelayTimer >= delayTillPlayJumpstart && hasPlayedJumpstart == false)
                {
                    if (PitchChangeEnabledJumpstart == true)
                    {
                        CurrentPitchJumpstart = Random.Range(MinPitchJumpstart, MaxPitchJumpstart);
                    }
                    FootstepAudioSource.clip = JumpstartSound;
                    FootstepAudioSource.pitch = CurrentPitchJumpstart;
                    FootstepAudioSource.Play();
                    hasPlayedJumpstart = true;
                }
            }
            else
            {
                if (PitchChangeEnabledLanding == true)
                {
                    CurrentPitchLanding = Random.Range(MinPitchLanding, MaxPitchLanding);
                }
                if (FallAffectsLandingVolume == true)
                {
                    // if airtime is equal to or above 3 (aprox time it takes to fall from max height???), then volume is max
                    //we have to add the difference between normal and max based on airtime
                    CurrentVolumeLanding = NormalVolumeLanding + (((MaxVolumeLanding - NormalVolumeLanding)/3)*JumpstartDelayTimer);
                }
                FootstepAudioSource.clip = LandingSounds[Random.Range(0, LandingSounds.Length)];
                FootstepAudioSource.pitch = CurrentPitchLanding;
                FootstepAudioSource.volume = CurrentVolumeLanding;
                FootstepAudioSource.Play();

                //have JumpstartDelayTimer decide volume of fall to a degree
                isJumping = false;
                hasPlayedJumpstart = false;
                JumpstartDelayTimer = 0;


            }
        }
        
        if (isDashing == true)
        {
            DashAudioSource.PlayOneShot(Dashsound, DashVolume);
            isDashing = false;
        }


    }

}
