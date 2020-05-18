using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    
    public GameObject projectilePrefab; 
    public GameObject projectileSpawn;

    public float detectionDistance;
    public float timeToShoot;
    public int health;

    float timeInRange;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDeath();
        CheckForLineOfSight();       
    }

    void CheckForDeath()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void CheckForLineOfSight()
    {
        //Shoot raycast in player's direction an check if he has been hit and the distance is not to far
        RaycastHit hit;
        Vector3 rayDirection = player.transform.position - transform.position;

        if (Physics.Raycast(transform.position, rayDirection, out hit, detectionDistance))
        {
            if (hit.transform == player.transform)
            {

                transform.LookAt(player.transform.position);

                //If the player is in range for a certain time --> shoot
                timeInRange += Time.deltaTime;
                if (timeInRange > timeToShoot)
                {
                    timeInRange = 0;
                    Instantiate(projectilePrefab, projectileSpawn.transform.position, transform.rotation);
                }
            }
            else
            {
                timeInRange = 0;
            }

        }
        else
        {
            timeInRange = 0;
        }
    }

   


}
