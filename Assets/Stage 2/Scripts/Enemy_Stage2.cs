using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stage2 : MonoBehaviour
{
    BoxCollider b;
    Bounds bounds;
    public int numberInSalve;
    public float delayInSalve;
    public float windup;
    public float fov;
    public float updatedFov;
    public Vector3 nextPoint;
    public float timeTillLost;
    bool detected;
    public float waitTillMove;

    public Vector3[] cornerPoints;
    // Start is called before the first frame update
    void Start()
    {
        b = GetComponentInParent<BoxCollider>();
        bounds = b.bounds;
        b.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = RandomPointInBounds(bounds);
        DetectionCheck();

    }

    public Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    void DetectionCheck(){


    }

}
