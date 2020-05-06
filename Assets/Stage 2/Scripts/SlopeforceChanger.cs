using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeforceChanger : MonoBehaviour
{
    // Start is called before the first frame update

    public float newSlopeForce;
    public float newSlopeForceRayLength;

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
            collision.gameObject.GetComponent<Moving_Stage2>().slopeForce = newSlopeForce;
            collision.gameObject.GetComponent<Moving_Stage2>().slopeForceRayLength = newSlopeForceRayLength;

            Destroy(gameObject);
        }
    }
}
