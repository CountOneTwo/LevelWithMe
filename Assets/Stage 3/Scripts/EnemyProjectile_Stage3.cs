﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Stage3 : MonoBehaviour
{
    public float projectileSpeed;
    public int damage;

    public float deviation;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 deviationVector = new Vector3(Random.Range(-deviation, deviation), Random.Range(-deviation, deviation), Random.Range(-deviation, deviation));


        transform.LookAt(GameObject.Find("Player").transform);
        //transform.rotation =  transform.rotation + Quaternion.Euler(Random.Range(-deviation, deviation) , Random.Range(-deviation, deviation), Random.Range(-deviation, deviation) );
        transform.eulerAngles += deviationVector;
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HealthAndRespawn_Stage3>() != null)
        {
            collision.gameObject.GetComponent<HealthAndRespawn_Stage3>().currentHealth -= damage;
            collision.gameObject.GetComponent<HealthAndRespawn_Stage3>().disappearTimer = 0;
            collision.gameObject.GetComponent<HealthAndRespawn_Stage3>().ActivateHealthBar();

            if (collision.gameObject.GetComponent<Regenaration_Stage3>() != null)
            {
                collision.gameObject.GetComponent<Regenaration_Stage3>().ManualFadeOut();
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<ShotgunEnemyStage2>() != null || collision.gameObject.GetComponent<EnemyProjectile_Stage3>() != null || collision.gameObject.GetComponent<Arrow_Stage3>() != null)
        {

        }
        else
        {
            Destroy(gameObject);
        }

    }
}
