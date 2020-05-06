using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeforceChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Moving_Stage2>() != null)
        {
           // collision.gameObject.GetComponent<Moving_Stage2>().currentHealth -= damage;
            
        }
    }
}
