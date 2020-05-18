using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAnimations : MonoBehaviour
{
    Animator animator;

    Moving_Stage3 movementScript;
    Shooting_Stage3 shootingScript;

    public GameObject arrow;

    void Start()
    {
        animator = GetComponent<Animator>();
        movementScript = GameObject.Find("Player").GetComponent<Moving_Stage3>();
        shootingScript = GameObject.Find("Main Camera").GetComponent<Shooting_Stage3>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update current drawstate (drawing, max draw, reloading etc.)
        animator.SetInteger("DrawState", shootingScript.drawState);

        //Disable the arrow if bow is released (arrow is shot) and reenable during reload animation
        if (shootingScript.drawState == 3)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
        }


        //Update to jumping animation if player is jumping
        animator.SetBool("Jumping", !movementScript.isGrounded);
        //Update to jumping animation if player is running
        animator.SetBool("Running", movementScript.activelyMoving);
        


    }
}
