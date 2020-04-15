using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject projectilePrefab;
    GameObject player;
    public float detectionDistance;
    public float timeToShoot;
    float timeInRange;
    public int health;
    public GameObject projectileSpawn;
    // Start is called before the first frame update
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
        RaycastHit hit;
        Vector3 rayDirection = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit, detectionDistance))
        {
            //Debug.Log("1");
            if (hit.transform == player.transform)
            {
                //Debug.Log("2");
                transform.LookAt(player.transform.position);
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
