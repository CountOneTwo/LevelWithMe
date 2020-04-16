using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float projectileSpeed;
    public int damage;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {      
         if (collision.gameObject.GetComponent<HealthAndRespawn>() != null)
        {
            collision.gameObject.GetComponent<HealthAndRespawn>().currentHealth -= damage;
            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<Enemy>() != null)
        {

        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
