using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stage2 : MonoBehaviour
{
    BoxCollider b;
    Bounds bounds;
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

    public Vector3[] cornerPoints;
    // Start is called before the first frame update
    void Start()
    {
        currentShot = 0;
        timeTillNextShot = windup;
        currentFov = fov;
       // b = GetComponentInParent<BoxCollider>();
       // bounds = b.bounds;
       // b.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = RandomPointInBounds(bounds);
        HealthCheck();
        DetectionCheck();
        if (detected)
        {
            DetectedActions();
        }
    }

    void HealthCheck()
    {
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    void DetectionCheck(){

        if (Vector3.Angle(transform.forward, GameObject.Find("Player").transform.position - transform.position) < currentFov) {
            print(Vector3.Angle(transform.forward, GameObject.Find("Player").transform.position));
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit)){
                print(hit.transform.gameObject.name);
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
            Instantiate(projectilePrefab, /*projectileSpawn.*/transform.position, transform.rotation);

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



}
