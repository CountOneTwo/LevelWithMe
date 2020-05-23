using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Stage2 : MonoBehaviour
{
    //Make respawn points invisible
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        foreach (MeshRenderer r in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = false;
        }
    }


    //Update the respawn point and orientation if trigger is entered - position and orientation are gotten from the red cube child to help the designers process
    void OnTriggerEnter(Collider collision)
    {        
        if (collision.gameObject.GetComponent<HealthAndRespawn_Stage2>() != null)
        {
            collision.gameObject.GetComponent<HealthAndRespawn_Stage2>().respawnPoint = transform.GetChild(0).transform.position;
            collision.gameObject.GetComponent<HealthAndRespawn_Stage2>().respawnOrientation = transform.GetChild(0).transform.eulerAngles;
        }
    }
}
