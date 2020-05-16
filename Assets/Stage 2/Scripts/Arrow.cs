using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;
    public float speed;
    public float mass;
    Rigidbody rb;
    public GameObject hitEnemyVFX;
    public GameObject hitWallVFX;


   /* public float projectileSpeed;
   
    public float maxDamage;
    public float maxProjectileSpeed;

    [HideInInspector]
    public float windupMultiplier;

    //public float speed;*/


    // Start is called before the first frame update
    void Start()
    {
        // transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y +90, transform.eulerAngles.z);
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, 5.0f);
        //  print(windupMultiplier);
        //  projectileSpeed = projectileSpeed * windupMultiplier;

        /*    if (projectileSpeed > maxProjectileSpeed)
            {
                projectileSpeed = maxProjectileSpeed;
            }*/

        /// GetComponent<Rigidbody>().velocity = transform.forward * speed * windupMultiplier;
        /// 
        rb.mass = mass;
      rb.velocity = transform.forward * speed;
        
       // damage = windupMultiplier * damage;

      /*  if (damage > maxDamage)
        {
            damage = maxDamage;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    void OnCollisionEnter(Collision collision)
    {
       // print(collision.gameObject.name);
        if (collision.gameObject.GetComponent<Enemy_Stage2>() != null)
        {
            collision.gameObject.GetComponent<Enemy_Stage2>().currentHealth -= damage;
            collision.gameObject.GetComponent<Enemy_Stage2>().detected = true;
            collision.gameObject.GetComponent<Enemy_Stage2>().timeUndetected = 0;
            Instantiate(hitEnemyVFX, transform.position, transform.rotation);

            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<ShotgunEnemyStage2>() != null)
        {
            collision.gameObject.GetComponent<ShotgunEnemyStage2>().currentHealth -= damage;
            collision.gameObject.GetComponent<ShotgunEnemyStage2>().detected = true;
            Instantiate(hitEnemyVFX, transform.position, transform.rotation);
        }
        else if (collision.gameObject.GetComponent<HealthAndRespawn>() != null)
        {

        }
        else
        {
            Instantiate(hitWallVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }

    void ArrowTestFunction()
    {
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;

        print(angle);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
