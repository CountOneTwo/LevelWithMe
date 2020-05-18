using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject projectileSpawn;

    void Update()
    {
        CheckForShooting();
    }

    //Shoot on mouse click if game is not paused
    void CheckForShooting()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.gamePaused)
        {
            Instantiate(projectilePrefab, projectileSpawn.transform.position, transform.rotation);
        }
    }
}
