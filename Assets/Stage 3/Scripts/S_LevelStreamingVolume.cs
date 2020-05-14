using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_LevelStreamingVolume : MonoBehaviour
{
    public GameObject[] ObjectsToLoad = new GameObject[3];
    public GameObject[] ObjectsToUnload = new GameObject[3];
    public bool HideOnPlay = true;

    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = !HideOnPlay;
    }

    private void OnTriggerEnter(Collider other)
    {
        S_PlayerStreaming ps;

        ps = other.gameObject.GetComponent<S_PlayerStreaming>();

        if (ps != null)
        {
            ps.LoadObjects(ObjectsToUnload, false);
            ps.LoadObjects(ObjectsToLoad, true);
        }
    }

}
