using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stage2 : MonoBehaviour
{
    //BoxCollider b;
    //Bounds bounds2;
    public int numberInSalve;
    public float delayInSalve;
    public float windup;
    public float fov;
    public float updatedFov;
    public Vector3 nextPoint;
    public float timeTillLost;

    [System.NonSerialized]
    public bool detected;

    public float waitTillMove;
    float currentFov;
    float timeTillNextShot;
    int currentShot;
    public GameObject projectileSpawn;
    public GameObject projectilePrefab;

    [System.NonSerialized]
    public float timeUndetected;

    public float health;
    public float allowedDeviation;

    public Vector3 centerOfSpawnArea;
    public Vector3 sizeOfSpawnArea;

    bool moving;
    float timeWaited = 0;

    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
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
                
            }

            transform.position = Vector3.MoveTowards(transform.position, nextPoint, movementSpeed * Time.deltaTime);
        }
        else
        {
            timeWaited += Time.deltaTime;
            if (timeWaited > waitTillMove)
            {
                timeWaited = 0;
                nextPoint = RandomPointInArea(centerOfSpawnArea, sizeOfSpawnArea);
                moving = true;

            }
        }
    }

    void HealthCheck()
    {
        if (health < 0)
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
            if (Physics.Raycast(transform.position, GameObject.Find("Player").transform.position - transform.position, out hit)){
              //  print(hit.transform.gameObject.name);
                if (hit.transform.gameObject.name.Equals("Player"))
                {
                    //Detected
                    detected = true;
                    timeUndetected = 0;
                    currentFov = updatedFov;
                    
                    return;
                }
            }
        }
        timeUndetected += Time.deltaTime;

        if (timeUndetected > timeTillLost)
        {
            detected = false;
            currentFov = fov;
            //Maybe reset time till next shot
          
        }

    }

    void DetectedActions()
    {
        transform.LookAt(GameObject.Find("Player").transform);
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
