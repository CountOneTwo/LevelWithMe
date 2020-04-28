using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float projectileSpeed;
    public float damage;
    public float maxDamage;
    public float maxProjectileSpeed;

    [HideInInspector]
    public float windupMultiplier;

    //public float speed;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f);
        print(windupMultiplier);
        projectileSpeed = projectileSpeed * windupMultiplier;

        if (projectileSpeed > maxProjectileSpeed)
        {
            projectileSpeed = maxProjectileSpeed;
        }

        GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed * windupMultiplier;

        damage = windupMultiplier * damage;

        if (damage > maxDamage)
        {
            damage = maxDamage;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
       // print(collision.gameObject.name);
        if (collision.gameObject.GetComponent<Enemy_Stage2>() != null)
        {
            collision.gameObject.GetComponent<Enemy_Stage2>().currentHealth -= damage;
            collision.gameObject.GetComponent<Enemy_Stage2>().detected = true;
            collision.gameObject.GetComponent<Enemy_Stage2>().timeUndetected = 0;


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

    void ArrowTestFunction()
    {
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;

        print(angle);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
