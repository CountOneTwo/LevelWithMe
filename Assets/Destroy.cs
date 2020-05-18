using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float TimeTillDestroy;
    private float Timer;

    //Destroys the gameobject after a certain time
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= TimeTillDestroy)
        {
            Destroy(gameObject);
        }
    }
}
