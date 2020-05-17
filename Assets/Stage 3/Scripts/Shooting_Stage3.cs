using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_Stage3 : MonoBehaviour
{
    [Header("General")]
    public GameObject arrowPrefab;
    public GameObject projectileSpawn;
    public float coolDown;
    float coolDownTimer = 500;

    [Header("Windup")]
    public float windupMaximum;
    public float windupMinimum;
    float windup;

    [Header("Damage")]
    public float minDamage;
    public float maxDamage;

    [Header("Speed")]
    public float minSpeed;
    public float maxSpeed;

    [Header("Mass")]
    public float minMass;
    public float maxMass;

    [Header("Crosshair")]
    public GameObject leftHalf;
    public GameObject rightHalf;
    public GameObject fullDrawn;
    public float returnTime;
    float returnTimer;

    [HideInInspector]
    public int drawState = 0;

    [Header("Draw Sound")]
    public AudioSource DrawSource;
    public AudioClip[] DrawClips;

    public bool PitchChangeEnabledDraw;
    [Range(-3, 3)] public float CurrentPitchDraw;
    [Range(-3, 3)] public float MinPitchDraw;
    [Range(-3, 3)] public float MaxPitchDraw;
    public bool VolumeChangeEnabledDraw;
    [Range(0, 1)] public float CurrentVolumeDraw;
    [Range(0, 1)] public float MinVolumeDraw;
    [Range(0, 1)] public float MaxVolumeDraw;

    [Header("Shoot Sound")]
    public AudioSource ShootSource;
    public AudioClip[] ShootClips;

    public bool PitchChangeEnabledShoot;
    [Range(-3, 3)] public float CurrentPitchShoot;
    [Range(-3, 3)] public float MinPitchShoot;
    [Range(-3, 3)] public float MaxPitchShoot;
    public bool VolumeChangeEnabledShoot;
    [Range(0, 1)] public float CurrentVolumeShoot;
    [Range(0, 1)] public float MinVolumeShoot;
    [Range(0, 1)] public float MaxVolumeShoot;

    public float ShootSoundEscalationSpeed;

    private bool isDrawing;
    private float TimeDrawHeld;
    private bool hasPlayedDrawSound;
    private bool justReleased;

    [Header("Full Charge Sound")]
    public AudioSource FCSource;
    public AudioClip[] FCClips;

    public bool PitchChangeEnabledFC;
    [Range(-3, 3)] public float CurrentPitchFC;
    [Range(-3, 3)] public float MinPitchFC;
    [Range(-3, 3)] public float MaxPitchFC;
    public bool VolumeChangeEnabledFC;
    [Range(0, 1)] public float CurrentVolumeFC;
    [Range(0, 1)] public float MinVolumeFC;
    [Range(0, 1)] public float MaxVolumeFC;

    private bool hasPlayedFC;

    [Header("Reload Sound")]
    public AudioSource ReloadSource;
    public AudioClip[] ReloadClips;
    public float delayfromM1release;

    public bool PitchChangeEnabledReload;
    [Range(-3, 3)] public float CurrentPitchReload;
    [Range(-3, 3)] public float MinPitchReload;
    [Range(-3, 3)] public float MaxPitchReload;
    public bool VolumeChangeEnabledReload;
    [Range(0, 1)] public float CurrentVolumeReload;
    [Range(0, 1)] public float MinVolumeReload;
    [Range(0, 1)] public float MaxVolumeReload;

    private bool isPlayingReload;
    private float releaseTimer;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer += Time.deltaTime;
        returnTimer += Time.deltaTime;
        if (!Regenaration_Stage3.regenerating && !HealthAndRespawn_Stage3.dead)
        {
            CheckForShooting();
        }
        

        CheckForCrosshairDisable();
        Sounds();
    }
    public void CheckForCrosshairDisable()
    {
        if (returnTimer > returnTime)
        {
            DisableCrosshair();
        }
    }

    public void DisableCrosshair()
    {
        if (returnTimer < returnTime)
            returnTimer = returnTime + 1;

        leftHalf.SetActive(false);
        rightHalf.SetActive(false);
    }

    public void ReturnCombatCrosshair()
    {
        fullDrawn.SetActive(false);
        leftHalf.SetActive(true);
        rightHalf.SetActive(true);
        leftHalf.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 0);
        rightHalf.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);
    }

    void CheckForShooting()
    {
        if (coolDownTimer > coolDown)
        {
            if (drawState == 3)
            {
                drawState = 0;
            }
            if (Input.GetMouseButton(0) && !GameManager.gamePaused)
            {
                float windupPercentage = windup / windupMaximum;

                leftHalf.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50 + (33 * windupPercentage), 0);
                rightHalf.GetComponent<RectTransform>().anchoredPosition = new Vector2(50 - (33 * windupPercentage), 0);
                // leftHalf.transform.position = new Vector2(-50 + (33 * windupPercentage), 0);
                // rightHalf.transform.position = new Vector2(50 - (33 * windupPercentage), 0);


                windup += Time.deltaTime;
                returnTimer = 0;
                isDrawing = true;
                if (windup > windupMaximum)
                {
                    leftHalf.SetActive(false);
                    rightHalf.SetActive(false);
                    fullDrawn.SetActive(true);
                    windup = windupMaximum;
                    drawState = 2;
                    FullChargeSound();
                }
                else
                {
                    drawState = 1;
                    leftHalf.SetActive(true);
                    rightHalf.SetActive(true);
                }
            }

            if (Input.GetMouseButtonUp(0) && !GameManager.gamePaused)
            {
                if (windup < windupMinimum)
                {
                    //windup = windupMinimum;
                    windup = 0;
                    return;
                }

                GameObject g = arrowPrefab;
                g.GetComponent<Arrow_Stage3>().damage = minDamage + ((windup / windupMaximum) * (maxDamage - minDamage));
                g.GetComponent<Arrow_Stage3>().speed = minSpeed + ((windup / windupMaximum) * (maxSpeed - minSpeed));
                g.GetComponent<Arrow_Stage3>().mass = maxMass - ((windup / windupMaximum) * (maxMass - minMass));


                ReturnCombatCrosshair();

                Instantiate(g, projectileSpawn.transform.position, transform.rotation);
                drawState = 3;
                windup = 0;
                coolDownTimer = 0;
                returnTimer = 0;
                isDrawing = false;
                hasPlayedDrawSound = false;
                justReleased = true;
                hasPlayedFC = false;
                isPlayingReload = true;
            }
        }

    }
    void Sounds()
    {
        if (isDrawing == true && hasPlayedDrawSound == false)
        {
            if (PitchChangeEnabledDraw == true)
            {
                CurrentPitchDraw = Random.Range(MinPitchDraw, MaxPitchDraw);
            }
            if (VolumeChangeEnabledDraw == true)
            {
                CurrentVolumeDraw = Random.Range(MinVolumeDraw, MaxVolumeDraw);
            }
            DrawSource.clip = DrawClips[Random.Range(0, DrawClips.Length)];
            DrawSource.pitch = CurrentPitchDraw;
            DrawSource.volume = CurrentVolumeDraw;
            DrawSource.Play();

            hasPlayedDrawSound = true;
        }

        if (isDrawing == true)
        {
            TimeDrawHeld += Time.deltaTime;
        }

        //we could base release pitch and volume on charge amount, maybe in stage 3 is better
        if (justReleased == true)
        {
            if (PitchChangeEnabledShoot == true)
            {
                CurrentPitchShoot = MinPitchShoot + TimeDrawHeld * ShootSoundEscalationSpeed;
                if (CurrentPitchShoot > MaxPitchShoot)
                {
                    CurrentPitchShoot = MaxPitchShoot;
                }
            }
            if (VolumeChangeEnabledShoot == true)
            {
                CurrentVolumeShoot = MinVolumeShoot + TimeDrawHeld * ShootSoundEscalationSpeed;
                if (CurrentVolumeShoot > MaxVolumeShoot)
                {
                    CurrentVolumeShoot = MaxVolumeShoot;
                }

            }
            ShootSource.clip = ShootClips[Random.Range(0, ShootClips.Length)];
            ShootSource.pitch = CurrentPitchShoot;
            ShootSource.volume = CurrentVolumeShoot;
            ShootSource.Play();

            justReleased = false;
            TimeDrawHeld = 0;
        }

        //Reload

        if (isPlayingReload == true)
        {
            releaseTimer += Time.deltaTime;
            if (releaseTimer >= delayfromM1release)
            {
                if (PitchChangeEnabledReload == true)
                {
                    CurrentPitchReload = Random.Range(MinPitchReload, MaxPitchReload);
                }
                if (VolumeChangeEnabledReload == true)
                {
                    CurrentVolumeReload = Random.Range(MinVolumeReload, MaxVolumeReload);
                }
                ReloadSource.clip = ReloadClips[Random.Range(0, ReloadClips.Length)];
                ReloadSource.pitch = CurrentPitchReload;
                ReloadSource.volume = CurrentVolumeReload;
                ReloadSource.Play();
                isPlayingReload = false;
                releaseTimer = 0;
            }
        }

    }
    void FullChargeSound()
    {
        if (hasPlayedFC == false)
        {
            if (PitchChangeEnabledFC == true)
            {
                CurrentPitchFC = Random.Range(MinPitchFC, MaxPitchFC);
            }
            if (VolumeChangeEnabledFC == true)
            {
                CurrentVolumeFC = Random.Range(MinVolumeFC, MaxVolumeFC);
            }
            FCSource.clip = FCClips[Random.Range(0, FCClips.Length)];
            FCSource.pitch = CurrentPitchFC;
            FCSource.volume = CurrentVolumeFC;
            FCSource.Play();
            hasPlayedFC = true;
        }
    }
   
}
