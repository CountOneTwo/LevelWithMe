using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunEnemyBoss : MonoBehaviour
{
    [Header("Movement")]
    public float waitTillMove;
    public float maxWaitTillMove;
    float currentWaitTillMove;
    public float allowedDeviation;
    [HideInInspector]
    public Vector3 nextPoint;
    public float rotationalSpeed;
    public float movementSpeed;

    [Header("Health")]
    public float maxHealth;
    public float healthPerTick;
    public float tickTime;
    public Slider healthSlider;
    public Canvas healthSliderCanvas;
    float tickTimer;
    [HideInInspector]
    public float currentHealth;
    public GameObject deathVFX;

    [Header("BossFight")]
    public Vector3 centerPoint;
    public float distanceFromCenterPoint;
    bool startingMovement = true;
    bool moving;
    float timeWaited = 0;

    [Header("Spawn")]
    public Vector3 centerOfSpawnArea;
    public Vector3 sizeOfSpawnArea;

    [Header("Detection")]
    public float fov;
    public float detectionDepth;

    [HideInInspector]
    public bool detected;

    [Header("Combat")]
    public float distanceToShoot;
    public float amountOfProjectiles;
    public float projectileConeAngle;
    public int projectileDamage;
    public float projectileSpeed;
    public float backwardsMoveSpeed;
    public float backwardsMoveDistance;
    public float waitTillChaseAgain;
    public float distanceToShowWindup;

    [Header("GameObjects")]
    public GameObject shotgunProjectiles;
    public GameObject windupVFX;

    bool movingBackwards;
    Vector3 backwardsDestination;
    float waitTillChaseAgainTimer;
    //Rigidbody rigidbody;
    CharacterController character;
    List<Vector3> nextChasePositon = new List<Vector3>();

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
    GameObject player;

  /* // public GameObject MusicManager;
    public musicManagerScript musicmanagerscript;
    [HideInInspector]
    public bool HasClaimedDetected;
    public bool HasClaimedUndetected;*/

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        windupVFX.SetActive(false);
        //rigidbody = GetComponent<Rigidbody>();
        character = GetComponent<CharacterController>();
        currentWaitTillMove = Random.Range(waitTillMove, maxWaitTillMove);
        Healthlastframe = maxHealth;
        currentHealth = maxHealth;
       // HasClaimedUndetected = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startingMovement)
        {
            StartMovement();
        }
        else
        {
            DetectionCheck();
            HealthCheck();
            if (detected)
            {
                DetectedActions();
            }
            else
            {
                Movement();

              /*  if (HasClaimedUndetected == false)
                {
                    musicmanagerscript.NrOfDetections -= 1;
                    HasClaimedUndetected = true;
                    HasClaimedDetected = false;
                }*/
            }
        }

        Sounds();
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
            Instantiate(DeathSound, gameObject.transform.position, transform.rotation);
            Instantiate(deathVFX, gameObject.transform.position, transform.rotation);
           /* if (HasClaimedDetected == true)
            {
                musicmanagerscript.NrOfDetections -= 1;
            }*/
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        GetComponentInParent<BossRoom>().currentEnemies--;
    }

    void StartMovement()
    {
        transform.LookAt(centerPoint);
        Vector3 heading = centerPoint - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        character.Move(direction * movementSpeed * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, centerPoint, movementSpeed);
        if (Vector3.Distance(transform.position, centerPoint) < distanceFromCenterPoint)
        {
            detected = true;
            startingMovement = false;
        }

    }

    void DetectedActions()
    {
        UpdateChasePositions();
       // print(nextChasePositon[0]);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(/*GameObject.Find("Player").transform.position*/nextChasePositon[0] - transform.position), Time.deltaTime * rotationalSpeed);
        if (!movingBackwards)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < distanceToShoot)
            {


                //Shoot projectiles
                for (int i = 0; i < amountOfProjectiles; i++)
                {
                    GameObject g = shotgunProjectiles;

                    g.GetComponent<EnemyProjectile_Stage3>().damage = projectileDamage;
                    g.GetComponent<EnemyProjectile_Stage3>().projectileSpeed = projectileSpeed;
                    g.GetComponent<EnemyProjectile_Stage3>().deviation = projectileConeAngle;


                    Instantiate(g, transform.position + transform.forward, transform.rotation);
                    Justshot = true;
                }
                //nextChasePositon.Clear();
                movingBackwards = true;
                backwardsDestination = transform.position - (transform.forward * backwardsMoveDistance);
                waitTillChaseAgainTimer = 0;
                windupVFX.SetActive(false);
            }
            else
            {
                if (Vector3.Distance(transform.position, player.transform.position) < distanceToShowWindup)
                {
                    windupVFX.SetActive(true);
                }

               // print(1);
                Vector3 heading = nextChasePositon[0] - transform.position;
                var distance = heading.magnitude;
                var direction = heading / distance;
                character.Move(direction * movementSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, nextChasePositon[0]) < 0.01)
                {
                    // print(nextChasePositon[0]);
                    nextChasePositon.RemoveAt(0);
                }


            }
        }
        else
        {
            if ((Vector3.Distance(transform.position, backwardsDestination) < 5))
            {
                waitTillChaseAgainTimer += Time.deltaTime;
                if (waitTillChaseAgainTimer > waitTillChaseAgain)
                {
                    movingBackwards = false;
                }
            }
            else
            {

                Vector3 heading = (backwardsDestination - transform.position).normalized;
                character.Move(heading * backwardsMoveSpeed * Time.deltaTime);

            }



        }
       /* if (HasClaimedDetected == false)
        {
            musicmanagerscript.NrOfDetections += 1;
            HasClaimedDetected = true;
            HasClaimedUndetected = false;
        }*/
    }

    void Movement()
    {
        if (moving)
        {
            if (Vector3.Distance(transform.position, nextPoint) < allowedDeviation)
            {
                moving = false;

            }

            transform.position = Vector3.MoveTowards(transform.position, nextPoint, movementSpeed * Time.deltaTime);
        }
        else
        {
            timeWaited += Time.deltaTime;
            if (timeWaited > currentWaitTillMove)
            {
                timeWaited = 0;
                nextPoint = RandomPointInArea(centerOfSpawnArea, sizeOfSpawnArea);
                moving = true;
                currentWaitTillMove = Random.Range(waitTillMove, maxWaitTillMove);
            }
        }
    }

    void UpdateChasePositions()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit))
        {
            //print(hit.transform.gameObject.name);
            if (hit.transform.gameObject.name.Equals("Player"))
            {
                nextChasePositon.Clear();
                nextChasePositon.Add(player.transform.position);
            }
            nextChasePositon.Add(player.transform.position);
        }
        else
        {
            for (int i = 0; i < nextChasePositon.Count; i++)
            {
                if (Physics.Raycast(nextChasePositon[i], player.transform.position - nextChasePositon[i], out hit))
                {
                    if (hit.transform.gameObject.name.Equals("Player"))
                    {
                        nextChasePositon.Insert(i + 1, player.transform.position);
                        //nextChasePositon.Add(GameObject.Find("Player").transform.position);
                        for (int x = i + 2; x < nextChasePositon.Count; x++)
                        {
                            //print(nextChasePositon[x]);
                            nextChasePositon.RemoveAt(x);

                        }
                        return;
                    }
                }
            }
        }

    }

    void PrepareChasePosition()
    {
        nextChasePositon.Clear();
        nextChasePositon.Add(player.transform.position);
    }


    void DetectionCheck()
    {
        //print(Vector3.Angle(transform.forward, GameObject.Find("Player").transform.position - transform.position));
        if (Vector3.Angle(transform.forward, player.transform.position - transform.position) < fov)
        {

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit) && Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < detectionDepth)
            {
                //  print(hit.transform.gameObject.name);
                if (hit.transform.gameObject.name.Equals("Player"))
                {
                    PrepareChasePosition();
                    //Detected
                    detected = true;
                }
            }
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

    void OnCollisionEnter(Collision collision)
    {
        if (movingBackwards)
        {
            backwardsDestination = transform.position;
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

        if (Justshot == true)
        {
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
