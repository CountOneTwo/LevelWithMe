using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Health")]
    public float maxHealth;
    public float healthPerTick;
    public float tickTime;
    public Slider healthSlider;
    public Canvas healthSliderCanvas;
    float tickTimer;
    [HideInInspector]
    public float currentHealth;



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
    //Rigidbody rigidbody;
    CharacterController character;
    List<Vector3> nextChasePositon = new List<Vector3>();




    // Start is called before the first frame update
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
       character = GetComponent<CharacterController>();
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

    void DetectedActions()
    {
        UpdateChasePositions();
        print(nextChasePositon[0]);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(/*GameObject.Find("Player").transform.position*/nextChasePositon[0] - transform.position), Time.deltaTime * rotationalSpeed);
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
                //nextChasePositon.Clear();
                movingBackwards = true;
                backwardsDestination = transform.position - (transform.forward * backwardsMoveDistance);
                waitTillChaseAgainTimer = 0;
            }
            else
            {
                print(1);
                Vector3 heading = nextChasePositon[0] -transform.position;
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
        if (Physics.Raycast(transform.position, GameObject.Find("Player").transform.position, out hit))
        {
            if (hit.transform.gameObject.name.Equals("Player"))
            {
                nextChasePositon.Clear();
                nextChasePositon.Add(GameObject.Find("Player").transform.position);
            }      
        }
        else
        {
            for (int i = 0; i < nextChasePositon.Count; i++)
            {
                if (Physics.Raycast(nextChasePositon[i], GameObject.Find("Player").transform.position - nextChasePositon[i], out hit))
                {
                    if (hit.transform.gameObject.name.Equals("Player"))
                    {
                        nextChasePositon.Insert(i+1, GameObject.Find("Player").transform.position);
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
        nextChasePositon.Add(GameObject.Find("Player").transform.position);
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

}
