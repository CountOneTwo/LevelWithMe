using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Stage2 : MonoBehaviour
{
    //BoxCollider b;
    //Bounds bounds2;

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
    [HideInInspector]
    public float currentHealth;

    


    [Header("Spawn")]
    public Vector3 centerOfSpawnArea;
    public Vector3 sizeOfSpawnArea;

    bool moving;
    float timeWaited = 0;



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

        

        if (currentHealth < 0)
        {
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
                    GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage2>().ActivateHealthBar();
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

        if (timeTillNextShot < 0)
        {
            Instantiate(projectilePrefab, /*projectileSpawn.*/transform.position + transform.forward, transform.rotation);

            if (currentShot == numberInSalve -1)
            {
                currentShot = 0;
                timeTillNextShot = windup;
            }
            else
            {
                currentShot++;
                timeTillNextShot = delayInSalve;
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centerOfSpawnArea, sizeOfSpawnArea);
    }
}
