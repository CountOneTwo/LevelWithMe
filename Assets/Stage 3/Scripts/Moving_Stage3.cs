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

    [HideInInspector]
    public bool dashing;
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

    //private AudioClip CurrentFootstep;
    private AudioClip RandomFootstep;
    private int NrOfFootstepsPlayed;
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

public float Airtime;
//Normal 1.2
//max 3.2ish
private bool hasPlayedJumpstart;
private bool isJumping;

[Header("Landing Sounds")]
public AudioClip[] LandingSounds;
public bool PitchChangeEnabledLanding;
[Range(-3, 3)] public float CurrentPitchLanding;
[Range(-3, 3)] public float MinPitchLanding;
[Range(-3, 3)] public float MaxPitchLanding;
public bool FallAffectsLandingVolume;
[Range(0, 1)] public float MinVolumeLanding;
[Range(0, 1)] public float NormalVolumeLanding;
[Range(0, 1)] public float MaxVolumeLanding;

public float CurrentVolumeLanding;
private bool GroundedLastFrame;


    [Header("Dash Sound")]
    public AudioSource DashAudioSource;
    public AudioClip Dashsound;
    [Range(0, 1)] public float DashVolume;

    private bool isDashing;

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
        Sounds();
        DashCooldown();
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

       // DashCooldown();

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
                dashGlow.SetActive(false);
                dashing = true;
                isDashing = true;
                cooldown = true;
                cooldownTimer = dashCooldown;
                dashProgression = 0;
                speedLines.SetActive(true);
                Dash();
                return;
            }
        }

       // DashCooldown();



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
        if (isGrounded == true)
        {
            if (IsMoving == true)
            {
                FootstepCooldownTimer += Time.deltaTime;

                if (FirstFootstep == true || FootstepCooldownTimer > FootstepCooldown)
                {
                    if (PitchChangeEnabledFootsteps == true)
                    {
                        CurrentPitchFootsteps = Random.Range(MinPitchFootsteps, MaxPitchFootsteps);
                    }
                    if (VolumeChangeEnabledFootsteps == true)
                    {
                        CurrentVolumeFootsteps = Random.Range(MinVolumeFootsteps, MaxVolumeFootsteps);
                    }
                    // FootstepAudioSource.clip = FootstepSounds[Random.Range(0, FootstepSounds.Length)];
                    FootstepAudioSource.clip = FootstepSounds[NrOfFootstepsPlayed];
                    FootstepAudioSource.pitch = CurrentPitchFootsteps;
                    FootstepAudioSource.volume = CurrentVolumeFootsteps;
                    FootstepAudioSource.Play();
                    NrOfFootstepsPlayed++;
                    if (NrOfFootstepsPlayed >= FootstepSounds.Length)
                    {
                        ShuffleArray();
                        NrOfFootstepsPlayed = 0;
                    }
                    FirstFootstep = false;
                    FootstepCooldownTimer = 0;
                }
            }
            else FirstFootstep = true;
        }
        //Jump start + landing
        
        if (isGrounded == false)
        {
            GroundedLastFrame = false;
            if (isJumping == true)
            {
                Airtime += Time.deltaTime;
                if (Airtime >= delayTillPlayJumpstart && hasPlayedJumpstart == false)
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
        }
        else if (GroundedLastFrame == false)
        {
            if (PitchChangeEnabledLanding == true)
            {
                CurrentPitchLanding = Random.Range(MinPitchLanding, MaxPitchLanding);
            }
            if (FallAffectsLandingVolume == true)
            {
                // CurrentVolumeLanding = NormalVolumeLanding + (((MaxVolumeLanding - NormalVolumeLanding)/3)*JumpstartDelayTimer);
                //CurrentVolumeLanding = NormalVolumeLanding + 3.2f - JumpstartDelayTimer;
                if (1.4f > Airtime && Airtime > 1.0f)
                {
                    CurrentVolumeLanding = NormalVolumeLanding;
                }
                else if (Airtime >= 1.4f)
                {
                    CurrentVolumeLanding = NormalVolumeLanding + (((MaxVolumeLanding - NormalVolumeLanding) / 3.2f) * Airtime);
                }
                else
                {
                    CurrentVolumeLanding = Airtime*NormalVolumeLanding;
                    if (CurrentVolumeLanding < MinVolumeLanding)
                    {
                        CurrentVolumeLanding = MinVolumeLanding;
                    }
                }
            }
            FootstepAudioSource.clip = LandingSounds[Random.Range(0, LandingSounds.Length)];
            FootstepAudioSource.pitch = CurrentPitchLanding;
            FootstepAudioSource.volume = CurrentVolumeLanding;
            FootstepAudioSource.Play();

            isJumping = false;
            GroundedLastFrame = true;
            hasPlayedJumpstart = false;
            Airtime = 0;
        }
        
        
        //Dashing Sound
        if (isDashing == true)
        {
            DashAudioSource.PlayOneShot(Dashsound, DashVolume);
            isDashing = false;
        }


    }
    private void ShuffleArray()
    {
        for (int i = 0; i < FootstepSounds.Length - 1; i++)
        {
            int rnd = Random.Range(i, FootstepSounds.Length);
            RandomFootstep = FootstepSounds[rnd];
            FootstepSounds[rnd] = FootstepSounds[i];
            FootstepSounds[i] = RandomFootstep;
        }
    }

}
