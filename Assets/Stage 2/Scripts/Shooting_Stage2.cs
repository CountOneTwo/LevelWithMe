using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_Stage2 : MonoBehaviour
{
    [Header("General")]
    public GameObject arrowPrefab;
    public GameObject projectileSpawn;
    public float coolDown;
    float coolDownTimer = 500;

   [Header("Windup")]
    public float windupMaximum;
    public float windupMinimum;
    float windup;

    [Header("Damage")]
    public float minDamage;
    public float maxDamage;

    [Header("Speed")]
    public float minSpeed;
    public float maxSpeed;

    [Header("Mass")]
    public float minMass;
    public float maxMass;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer += Time.deltaTime;
        CheckForShooting();
    }

    void CheckForShooting()
    {
        if (coolDownTimer > coolDown)
        {
            if (Input.GetMouseButton(0) && !GameManager.gamePaused)
            {
                windup += Time.deltaTime;

                if (windup > windupMaximum)
                {
                    windup = windupMaximum;
                }
            }

            if (Input.GetMouseButtonUp(0) && !GameManager.gamePaused)
            {
                if (windup < windupMinimum)
                {
                    //windup = windupMinimum;
                    windup = 0;
                    return;
                }

                GameObject g = arrowPrefab;
                g.GetComponent<Arrow>().damage = minDamage + ((windup / windupMaximum) * (maxDamage - minDamage));
                g.GetComponent<Arrow>().speed = minSpeed + ((windup / windupMaximum) * (maxSpeed - minSpeed));
                g.GetComponent<Arrow>().mass = maxMass - ((windup / windupMaximum) * (maxMass - minMass));




                Instantiate(g, projectileSpawn.transform.position, transform.rotation);
                windup = 0;
                coolDownTimer = 0;
            }
        }

    }
}
