using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Stage2 : MonoBehaviour
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
        print(1);
        if (collision.gameObject.GetComponent<HealthAndRespawn_Stage2>() != null)
        {
            collision.gameObject.GetComponent<HealthAndRespawn_Stage2>().respawnPoint = transform.GetChild(0).transform.position;
            collision.gameObject.GetComponent<HealthAndRespawn_Stage2>().respawnOrientation = transform.GetChild(0).transform.eulerAngles;
        }
    }
}
