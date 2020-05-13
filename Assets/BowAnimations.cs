using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAnimations : MonoBehaviour
{
    Animator animator;
    Moving_Stage3 movementScript;
    Shooting_Stage3 shootingScript;
    //Vector3 positionLastFrame;
    public GameObject arrow;
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
        if (shootingScript.drawState == 3)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
        }



        animator.SetBool("Jumping", !movementScript.isGrounded);


            animator.SetBool("Running", movementScript.activelyMoving);
        


    }
}
