using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject projectileSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForShooting();
    }

    void CheckForShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Instantiate(projectilePrefab, projectileSpawn.transform.position, transform.rotation);
        }
    }
}
