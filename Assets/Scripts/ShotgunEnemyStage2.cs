using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunEnemyStage2 : MonoBehaviour
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

    [Header("GameObjects")]
    public GameObject shotgunProjectiles;

    bool movingBackwards;
    Vector3 backwardsDestination;
    float waitTillChaseAgainTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentWaitTillMove = Random.Range(waitTillMove, maxWaitTillMove);
    }

    // Update is called once per frame
    void Update()
    {
        DetectionCheck();
        if (detected)
        {
            DetectedActions();
        }
        else
        {
            Movement();
        }
        
    }

    void DetectedActions()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GameObject.Find("Player").transform.position - transform.position), Time.deltaTime * rotationalSpeed);
        if (!movingBackwards)
        {
            if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < distanceToShoot)
            {
                //Shoot projectiles
                for (int i = 0; i < amountOfProjectiles; i++)
                {
                    GameObject g = shotgunProjectiles;
                  
                    g.GetComponent<EnemyProjectile_Stage3>().damage = projectileDamage;
                    g.GetComponent<EnemyProjectile_Stage3>().projectileSpeed = projectileSpeed;
                    g.GetComponent<EnemyProjectile_Stage3>().deviation = projectileConeAngle;
                   

                    Instantiate(g, transform.position + transform.forward, transform.rotation);
                }
                movingBackwards = true;
                backwardsDestination = transform.position - transform.forward * backwardsMoveDistance;
            }
            else
            {

                
                transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, movementSpeed * Time.deltaTime);
            }
        }
        else
        {
            if ((Vector3.Distance(transform.position, backwardsDestination) < 0.01))
            {
                waitTillChaseAgainTimer += Time.deltaTime;
                if (waitTillChaseAgainTimer > waitTillChaseAgain)
                {
                    movingBackwards = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, backwardsDestination, backwardsMoveSpeed * Time.deltaTime);
                waitTillChaseAgainTimer = 0;
            }
           


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
            if (timeWaited > currentWaitTillMove)
            {
                timeWaited = 0;
                nextPoint = RandomPointInArea(centerOfSpawnArea, sizeOfSpawnArea);
                moving = true;
                currentWaitTillMove = Random.Range(waitTillMove, maxWaitTillMove);
            }
        }
    }

    void DetectionCheck()
    {
        //print(Vector3.Angle(transform.forward, GameObject.Find("Player").transform.position - transform.position));
        if (Vector3.Angle(transform.forward, GameObject.Find("Player").transform.position - transform.position) < fov)
        {

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, GameObject.Find("Player").transform.position - transform.position, out hit) && Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < detectionDepth)
            {
                //  print(hit.transform.gameObject.name);
                if (hit.transform.gameObject.name.Equals("Player"))
                {
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

    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centerOfSpawnArea, sizeOfSpawnArea);
    }

}
