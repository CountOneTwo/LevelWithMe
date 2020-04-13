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
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(3);
        if(collision.gameObject.GetComponent<Enemy>() != null)
        {
            Debug.Log(4);
            collision.gameObject.GetComponent<Enemy>().health -= damage;
        }
        else if (collision.gameObject.GetComponent<HealthAndRespawn>() != null)
        {
            Debug.Log(5);
            collision.gameObject.GetComponent<HealthAndRespawn>().currentHealth -= damage;
        }
        Destroy(gameObject);
    }
}
