using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAnimations : MonoBehaviour
{
    Animator animator;
    Moving_Stage3 movementScript;
    Shooting_Stage3 shootingScript;
    Vector3 positionLastFrame;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movementScript = GameObject.Find("Player").GetComponent<Moving_Stage3>();
        shootingScript = GameObject.Find("Main Camera").GetComponent<Shooting_Stage3>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("DrawState", shootingScript.drawState);
        animator.SetBool("Jumping", !movementScript.isGrounded);

        if (positionLastFrame == transform.position)
        {
            animator.SetBool("Running", false);
        }
        else
        {
            animator.SetBool("Running", true);
        }


        positionLastFrame = transform.position;
    }
}
