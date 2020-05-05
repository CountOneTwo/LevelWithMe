using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Stage2 : MonoBehaviour
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
        if (collision.gameObject.GetComponent<HealthAndRespawn_Stage2>() != null)
        {
            collision.gameObject.GetComponent<HealthAndRespawn_Stage2>().currentHealth -= damage;
            collision.gameObject.GetComponent<HealthAndRespawn_Stage2>().ActivateHealthBar();

            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<Enemy_Stage2>() != null)
        {

        }
        else
        {
            Destroy(gameObject);
        }

    }
}
