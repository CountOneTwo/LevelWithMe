using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public GameObject doors;
    public Vector3 doorPosition;
    public Vector3 originalDoorPositon;
    public float doorSpeed;
    bool activated;
    bool doorsClosed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!doorsClosed && activated)
        {
            CloseDoors();
        }
    }

    void CloseDoors()
    {
        if (Vector3.Distance(doorPosition, doors.transform.position) < 0.5)
        {
            doors.transform.position -= (-transform.up * doorSpeed * Time.deltaTime);
            doorsClosed = true;
        }
       
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!activated)
        {
            if (collision.gameObject.name.Equals("Player"))
            {
                activated = true;
            }
        }

    }

}
