using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTile : MonoBehaviour
{
    Vector3 originalPoisition;
    public MovementTimeAndDestination[] movementTimeAndDestinations;
    MeshRenderer renderer;
    public Material alternativeMaterial;
    public Material originalMaterial;
    BossRoom bossRoom;
   // BoxCollider collider;
   // BoxCollider parentCollider;
    Transform parentTransform;
    int currentMovement;
    public float jiggleTime;
    public float jiggleDistance;
    ///public float jiggleShiftDuration;
    //float jiggleShiftTimer;
    bool jiggleUpwards;
    float jiggleTimer;
    public float jiggleSpeed;
    public float movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
       // parentCollider = GetComponentInParent<BoxCollider>();
        currentMovement = 0;
       // collider = GetComponent<BoxCollider>();
        originalPoisition = transform.position;
        renderer = GetComponent<MeshRenderer>();
        bossRoom = GameObject.Find("Boss Room Trigger").GetComponent<BossRoom>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossRoom.durationTimer <= 0)
        {
            Reset();
        }

        CheckForMovement();
    }

    void Reset()
    {
        transform.position = originalPoisition;
        currentMovement = 0;
        renderer.material = originalMaterial;
    }

    void CheckForMovement()
    {
        if (currentMovement < movementTimeAndDestinations.Length)
        {
            if (bossRoom.durationTimer >= movementTimeAndDestinations[currentMovement].movementTime)
            {
                jiggleTimer += Time.deltaTime;
                //Start jiggling
                if (renderer.material != alternativeMaterial)
                    renderer.material = alternativeMaterial;




                if (jiggleTimer < jiggleTime)
                {
                    //jiggleShiftTimer += Time.deltaTime;
                    
                    //transform.position +=
                    if (jiggleUpwards)
                    {
                        ///float jiggleAmount = jiggleSpeed * Time.deltaTime;
                        //transform.position += new Vector3(0, jiggleAmount, 0);
                        //collider.center.Set(0, collider.center.y - jiggleAmount, 0);
                        //collider.center = collider.center - new Vector3(0, jiggleAmount, 0);
                        transform.position = Vector3.MoveTowards(transform.position, parentTransform.position + new Vector3(0,jiggleDistance,0), jiggleSpeed * Time.deltaTime);
                        if (Vector3.Distance(transform.position, parentTransform.position + new Vector3(0, jiggleDistance, 0)) < 0.05)
                        {
                            jiggleUpwards = !jiggleUpwards;
                        }
                    }
                    else
                    {
                        //float jiggleAmount = jiggleSpeed * Time.deltaTime;
                        //transform.position -= new Vector3(0, jiggleAmount, 0);
                        //collider.center.Set(0, collider.center.y + jiggleAmount, 0);
                        //collider.center = collider.center + new Vector3(0, jiggleAmount, 0);
                        transform.position = Vector3.MoveTowards(transform.position, parentTransform.position - new Vector3(0, jiggleDistance, 0), jiggleSpeed * Time.deltaTime);
                        if (Vector3.Distance(transform.position, parentTransform.position - new Vector3(0, jiggleDistance, 0)) < 0.05)
                        {
                            jiggleUpwards = !jiggleUpwards;
                        }
                    }

                   /* if (jiggleShiftTimer > jiggleShiftDuration)
                    {
                        jiggleUpwards = !jiggleUpwards;
                    }*/

                }
                else
                {
                    transform.localPosition = Vector3.zero;
                    if (Vector3.Distance(parentTransform.position, movementTimeAndDestinations[currentMovement].destinationPosition) > 0.1)
                    {
                        parentTransform.position = Vector3.MoveTowards(parentTransform.position, movementTimeAndDestinations[currentMovement].destinationPosition, movementSpeed * Time.deltaTime);
                    }
                    else
                    {
                        currentMovement++;
                        jiggleTimer = 0;
                        renderer.material = originalMaterial;
                    }
                }

            }
        }
        
    }
}
