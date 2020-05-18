using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
        //Move the projectile forward
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
    }

    //Check for collisions and if it's an enemy subtract health
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null)
        {
            collision.gameObject.GetComponent<Enemy>().health -= damage;
            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<HealthAndRespawn>() != null)
        {
            
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

}
