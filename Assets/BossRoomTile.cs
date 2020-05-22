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

    float[] finalDestinationPositions;
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

        finalDestinationPositions = new float[movementTimeAndDestinations.Length];
        for (int i = 0; i < movementTimeAndDestinations.Length; i++)
        {
            float f = parentTransform.position.y;
            for (int j = 0; j <= i; j++)
            {
                f += movementTimeAndDestinations[j].destinationPosition;
            }


            finalDestinationPositions[i] = f;
        }

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
                    if (Vector3.Distance(parentTransform.position, new Vector3(parentTransform.position.x,finalDestinationPositions[currentMovement], parentTransform.position.z)) > 0.1)
                    {
                        parentTransform.position = Vector3.MoveTowards(parentTransform.position, new Vector3(parentTransform.position.x, finalDestinationPositions[currentMovement], parentTransform.position.z), movementSpeed * Time.deltaTime);
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
