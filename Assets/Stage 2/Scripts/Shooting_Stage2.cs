using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_Stage2 : MonoBehaviour
{

    public GameObject arrowPrefab;
    public GameObject projectileSpawn;
    float windup;
    public float windupMaximum;
    public float windupMinimum;
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
        if (Input.GetMouseButton(0) && !GameManager.gamePaused)
        {
            windup += Time.deltaTime;

            if (windup> windupMaximum)
            {
                windup = windupMaximum;
            }
        }

        if (Input.GetMouseButtonUp(0) && !GameManager.gamePaused)
        {
            if (windup < windupMinimum)
            {
                windup = windupMinimum;
            }

            GameObject g = arrowPrefab;
            g.GetComponent<Arrow>().windupMultiplier = windup;
            Instantiate(g, projectileSpawn.transform.position, transform.rotation);
            windup = 0;
        }
    }
}
