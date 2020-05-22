using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTile : MonoBehaviour
{
    Vector3 originalPoisition;
    public MovementTimeAndDestination[] movementTimeAndDestinations;
    Material currentMaterial;
    public Material alternativeMaterial;
    public Material originalMaterial;
    BossRoom bossRoom;
    BoxCollider collider;
    int currentMovement;
    public float jiggleTime;
    public float jiggleShiftDuration;
    float jiggleShiftTimer;
    bool jiggleUpwards;
    float jiggleTimer;
    public float jiggleSpeed;
    public float movementSpeed;
    // Start is called before the first frame update
    void Start()
    {

        currentMovement = 0;
        collider = GetComponent<BoxCollider>();
        originalPoisition = transform.position;
        currentMaterial = GetComponent<Material>();
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
        currentMaterial = originalMaterial;
    }

    void CheckForMovement()
    {
        if (bossRoom.durationTimer >= movementTimeAndDestinations[currentMovement].movementTime)
        {
            jiggleTimer += Time.deltaTime;
            //Start jiggling
            if (currentMaterial == originalMaterial)
                currentMaterial = alternativeMaterial;

            


            if (jiggleTimer < jiggleTime)
            {
                jiggleShiftTimer += Time.deltaTime;
                //transform.position +=
                if (jiggleUpwards)
                {
                    float jiggleAmount = jiggleSpeed * Time.deltaTime;
                    transform.position += new Vector3(0, jiggleAmount, 0);
                    collider.center.Set(0,collider.center.y - jiggleAmount,0);
                }
                else
                {
                    float jiggleAmount = jiggleSpeed * Time.deltaTime;
                    transform.position -= new Vector3(0, jiggleAmount, 0);
                    collider.center.Set(0, collider.center.y + jiggleAmount, 0);
                }

                if (jiggleShiftTimer > jiggleShiftDuration)
                {
                    jiggleUpwards = !jiggleUpwards;
                }

            }
            else
            {
                if (Vector3.Distance(transform.position, movementTimeAndDestinations[currentMovement].destinationPosition) > 0.1)
                {
                    transform.position = Vector3.MoveTowards(transform.position, movementTimeAndDestinations[currentMovement].destinationPosition, movementSpeed * Time.deltaTime);
                }
                else
                {
                    currentMovement++;
                    jiggleTimer = 0;
                    currentMaterial = originalMaterial;
                }
            }

        }
    }
}
