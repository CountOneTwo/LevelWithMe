using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateHealthAndShooting : MonoBehaviour
{
    public Shooting shooting;
    public Text healthCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Player"))
        {
            shooting.enabled = true;
            healthCounter.enabled = true;
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.name.Equals("Player"))
        {
            shooting.enabled = true;
            healthCounter.enabled = true;
        }
    }
}
