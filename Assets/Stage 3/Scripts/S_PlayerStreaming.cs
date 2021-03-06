﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerStreaming : MonoBehaviour
{
    public GameObject[] ObjectsToLoadOnStart = new GameObject[1];
    public GameObject[] ObjectsToUnloadOnStart = new GameObject[1];

    public bool DisableLevelStreaming = false;
    public bool DisableLoadOnStart = false;

    void Start()
    {
        if (DisableLevelStreaming || DisableLoadOnStart)
        {

        }
        else
        {
            LoadObjectsOnStart();
        }
    }

    public void LoadObjectsOnStart()
    {
        LoadObjects(ObjectsToUnloadOnStart, false);
        LoadObjects(ObjectsToLoadOnStart, true);
    }

    public void LoadObjects(GameObject[] gObjects, bool load)
    {
        if(!DisableLevelStreaming)
        {
            foreach (GameObject o in gObjects)
            {
                if (o != null)
                {
                    if (o.activeInHierarchy != load)
                    {
                        o.SetActive(load);
                    }
                }
            }
        }
    }
}
