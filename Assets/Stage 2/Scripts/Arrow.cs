using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float projectileSpeed;
    public int damage;
    public float windupMultiplier;

    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f);
        GetComponent<Rigidbody>().velocity = transform.forward * speed * windupMultiplier;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
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

    void ArrowTestFunction()
    {
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;

        print(angle);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
