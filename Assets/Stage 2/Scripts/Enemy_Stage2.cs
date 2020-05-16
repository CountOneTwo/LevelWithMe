﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Stage2 : MonoBehaviour
{
    //BoxCollider b;
    //Bounds bounds2;

    public bool Stage3;

    [Header("Shooting")]
    public int numberInSalve;
    public float delayInSalve;
    public float windup;
    
   // public float deviationScaler;

    [Header("Detection")]
    public float fov;
    public float updatedFov;
    public float detectionDepth;
    public float updatedDetectionDepth;
    public float timeTillLost;


    [Header("Movement")]
    public float waitTillMove;
    public float maxWaitTillMove;
    float currentWaitTillMove;
    public float allowedDeviation;
    [HideInInspector]
    public Vector3 nextPoint;
    public float rotationalSpeed;
    public float idleRotationalSpeed;
    public float movementSpeed;
    public bool lookInDirection;


    [HideInInspector]
    public bool detected;



    float currentFov;
    float currentDetectionDepth;

    float timeTillNextShot;
    int currentShot;

   // public GameObject projectileSpawn;
    public GameObject projectilePrefab;

    [HideInInspector]
    public float timeUndetected;


    [Header("Health")]
    public float maxHealth;
    public float healthPerTick;
    public float tickTime;
    public Slider healthSlider;
    public Canvas healthSliderCanvas;
    float tickTimer;
    public GameObject deathVFX;
    [HideInInspector]
    public float currentHealth;

    


    [Header("Spawn")]
    public Vector3 centerOfSpawnArea;
    public Vector3 sizeOfSpawnArea;

    bool moving;
    float timeWaited = 0;

    [Header("Taking Damage Sound")]
    public AudioSource TakeDamageSource;
    public AudioClip TakeHighDamageClip;
    public AudioClip TakeLowDamageClip;

    public float HighDamageThreshold;

    [Range(0, 1)] public float MaxHighDamageVolume;
    public bool PitchChangeEnabledHighDamage;
    [Range(-3, 3)] public float CurrentPitchHighDamage;
    [Range(-3, 3)] public float MinPitchHighDamage;
    [Range(-3, 3)] public float MaxPitchHighDamage;

    [Range(0, 1)] public float MaxLowDamageVolume;
    public bool PitchChangeEnabledLowDamage;
    [Range(-3, 3)] public float CurrentPitchLowDamage;
    [Range(-3, 3)] public float MinPitchLowDamage;
    [Range(-3, 3)] public float MaxPitchLowDamage;

    private float Healthlastframe;

    [Header("Shooting Sound")]
    public AudioSource ShootingSource;
    public AudioClip[] ShootingClips;

    public bool PitchChangeEnabledShooting;
    [Range(-3, 3)] public float CurrentPitchShooting;
    [Range(-3, 3)] public float MinPitchShooting;
    [Range(-3, 3)] public float MaxPitchShooting;
    public bool VolumeChangeEnabledShooting;
    [Range(0, 1)] public float CurrentVolumeShooting;
    [Range(0, 1)] public float MinVolumeShooting;
    [Range(0, 1)] public float MaxVolumeShooting;

    private bool Justshot;

    [Header("Windup Sound")]
    public AudioSource WindUpSource;
    public AudioClip[] WindUpClips;

    public bool PitchChangeEnabledWindUp;
    [Range(-3, 3)] public float CurrentPitchWindUp;
    [Range(-3, 3)] public float MinPitchWindUp;
    [Range(-3, 3)] public float MaxPitchWindUp;
    public bool VolumeChangeEnabledWindUp;
    [Range(0, 1)] public float CurrentVolumeWindUp;
    [Range(0, 1)] public float MinVolumeWindUp;
    [Range(0, 1)] public float MaxVolumeWindUp;

    private bool hasPlayedWindUp;
    public float PlaySecondsBeforeShot = .5f;
    private bool resettedmagazine = true;

    public GameObject DeathSound;

    // Start is called before the first frame update
    void Start()
    {
        currentWaitTillMove = Random.Range(waitTillMove, maxWaitTillMove);
        currentDetectionDepth = detectionDepth;
        currentHealth = maxHealth;
        currentShot = 0;
        timeTillNextShot = windup;
        currentFov = fov;
        // b = GetComponentInParent<BoxCollider>();
        // bounds2 = b.bounds;
        //print(bounds2);
        // b.enabled = false;
        nextPoint = RandomPointInArea(centerOfSpawnArea, sizeOfSpawnArea);
        Healthlastframe = maxHealth;
        hasPlayedWindUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        //  print(bounds2);
        // transform.position = RandomPointInArea(centerOfSpawnArea, sizeOfSpawnArea);
        // print(RandomPointInBounds(bounds));
        //transform.position = new Vector3(0,0,0);
        
        Movement();
        HealthCheck();
        DetectionCheck();
        if (detected)
        {
            DetectedActions();
        }
        Sounds();
    }

    void Movement()
    {
        if (moving)
        {
            if (Vector3.Distance(transform.position, nextPoint) < allowedDeviation)
            {
                moving = false;
                nextPoint = RandomPointInArea(centerOfSpawnArea, sizeOfSpawnArea);
            }
            if (!detected && lookInDirection == true)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(nextPoint - transform.position), Time.deltaTime * idleRotationalSpeed);
            }
           

            transform.position = Vector3.MoveTowards(transform.position, nextPoint, movementSpeed * Time.deltaTime);
        }
        else
        {
            if (!detected && lookInDirection == true)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(nextPoint - transform.position), Time.deltaTime * idleRotationalSpeed);
            }
            
            timeWaited += Time.deltaTime;
            if (timeWaited > currentWaitTillMove)
            {
                timeWaited = 0;
                
                moving = true;
                currentWaitTillMove = Random.Range(waitTillMove, maxWaitTillMove);
            }
        }
    }

    void HealthCheck()
    {
        healthSlider.value = currentHealth / maxHealth;
        //print(currentHealth);
        if (currentHealth < maxHealth)
        {
            healthSliderCanvas.enabled = true;
            tickTimer += Time.deltaTime;
        }
        //healthSlider.value = currentHealth / maxHealth;


        if (!detected)
        {
            if (tickTimer > tickTime)
            {
                currentHealth += healthPerTick;
                tickTimer = 0;
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
            }
        }

        

        if (currentHealth <= 0)
        {
            if (Stage3 == true)
            {
                Instantiate(DeathSound, gameObject.transform.position, transform.rotation);
                Instantiate(deathVFX, gameObject.transform.position, transform.rotation);

            }



            Destroy(gameObject);
        }
    }

    public Vector3 RandomPointInArea(Vector3 center, Vector3 size)
    {
        return new Vector3(
            Random.Range(center.x - size.x / 2, center.x + size.x / 2),
            Random.Range(center.y - size.y / 2, center.y + size.y / 2),
            Random.Range(center.z - size.z / 2, center.z + size.z / 2)
        );
    }

    void DetectionCheck(){
        //print(Vector3.Angle(transform.forward, GameObject.Find("Player").transform.position - transform.position));
        if (Vector3.Angle(transform.forward, GameObject.Find("Player").transform.position - transform.position) < currentFov) {
           
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, GameObject.Find("Player").transform.position - transform.position, out hit) && Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < currentDetectionDepth)
            {
              //  print(hit.transform.gameObject.name);
                if (hit.transform.gameObject.name.Equals("Player"))
                {
                    //Detected
                    detected = true;
                    timeUndetected = 0;
                    currentFov = updatedFov;
                    currentDetectionDepth = updatedDetectionDepth;
                    if (GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage2>() != null)
                    {
                        GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage2>().ActivateHealthBar();
                    }
                    else if (GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage3>() != null)
                    {
                        GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage3>().disappearTimer = 0;
                        GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage3>().ActivateHealthBar();
                        
                    }


                    
                    
                    return;
                }
            }
        }
        timeUndetected += Time.deltaTime;

        if (timeUndetected > timeTillLost)
        {
            detected = false;
            currentFov = fov;
            currentDetectionDepth = detectionDepth;
            //Maybe reset time till next shot
          
        }

    }

    void DetectedActions()
    {
        //transform.LookAt(GameObject.Find("Player").transform);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GameObject.Find("Player").transform.position - transform.position), Time.deltaTime * rotationalSpeed);


        timeTillNextShot -= Time.deltaTime;
        if (timeTillNextShot <= PlaySecondsBeforeShot && resettedmagazine == true && Stage3 == true)
        {
            hasPlayedWindUp = false;
            resettedmagazine = false;
        }
        if (timeTillNextShot < 0)
        {
            Instantiate(projectilePrefab, /*projectileSpawn.*/transform.position + transform.forward, transform.rotation);
            Justshot = true;

            if (currentShot == numberInSalve -1)
            {
                currentShot = 0;
                timeTillNextShot = windup;
            }
            else
            {
                currentShot++;
                timeTillNextShot = delayInSalve;
                resettedmagazine = true;
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centerOfSpawnArea, sizeOfSpawnArea);
    }

    void Sounds()
    {
        if (currentHealth < Healthlastframe)
        {
            if ((Healthlastframe - currentHealth) > HighDamageThreshold)
            {
                if (PitchChangeEnabledHighDamage == true)
                {
                    CurrentPitchHighDamage = Random.Range(MinPitchHighDamage, MaxPitchHighDamage);
                }
                TakeDamageSource.clip = TakeHighDamageClip;
                TakeDamageSource.volume = MaxHighDamageVolume;
                TakeDamageSource.pitch = CurrentPitchHighDamage;
                TakeDamageSource.Play();
            }
            else
            {
                if (PitchChangeEnabledLowDamage == true)
                {
                    CurrentPitchLowDamage = Random.Range(MinPitchLowDamage, MaxPitchLowDamage);
                }
                TakeDamageSource.clip = TakeLowDamageClip;
                TakeDamageSource.volume = MaxLowDamageVolume;
                TakeDamageSource.pitch = CurrentPitchLowDamage;
                TakeDamageSource.Play();
            }


        }
        Healthlastframe = currentHealth;

        if (Justshot == true) {
            if (PitchChangeEnabledShooting == true)
            {
                CurrentPitchShooting = Random.Range(MinPitchShooting, MaxPitchShooting);
            }
            if (VolumeChangeEnabledShooting == true)
            {
                CurrentVolumeShooting = Random.Range(MinVolumeShooting, MaxVolumeShooting);
            }
            ShootingSource.clip = ShootingClips[Random.Range(0, ShootingClips.Length)];
            ShootingSource.pitch = CurrentPitchShooting;
            ShootingSource.volume = CurrentVolumeShooting;
            ShootingSource.Play();
            Justshot = false;
        }

        if (hasPlayedWindUp == false)
        {
            if (PitchChangeEnabledWindUp == true)
            {
                CurrentPitchWindUp = Random.Range(MinPitchWindUp, MaxPitchWindUp);
            }
            if (VolumeChangeEnabledWindUp == true)
            {
                CurrentVolumeWindUp = Random.Range(MinVolumeWindUp, MaxVolumeWindUp);
            }
            WindUpSource.clip = WindUpClips[Random.Range(0, WindUpClips.Length)];
            WindUpSource.pitch = CurrentPitchWindUp;
            WindUpSource.volume = CurrentVolumeWindUp;
            WindUpSource.Play();
            hasPlayedWindUp = true;
        }
    }

}
