using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject projectileSpawn;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        CheckForShooting();
    }

    void CheckForShooting()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.gamePaused)
        {
            Instantiate(projectilePrefab, projectileSpawn.transform.position, transform.rotation);
        }
    }
}
