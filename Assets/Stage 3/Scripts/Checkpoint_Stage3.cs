using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Stage3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        foreach (MeshRenderer r in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = false;
        }
    }


    void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.GetComponent<HealthAndRespawn_Stage3>() != null)
        {
            collision.gameObject.GetComponent<HealthAndRespawn_Stage3>().respawnPoint = transform.GetChild(0).transform.position;
            collision.gameObject.GetComponent<HealthAndRespawn_Stage3>().respawnOrientation = transform.GetChild(0).transform.eulerAngles;
        }
    }
}
