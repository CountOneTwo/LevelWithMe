﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthAndRespawn_QAStage : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public Vector3 respawnPoint;
    public int currentHealth;
    [SerializeField] private float minimumHeight;
    public Text healthCounter;

    private CharacterController charController;


    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(0,0,0);
        healthCounter.text = "Health: " + currentHealth;
        CheckForRespawn();
        CheckForOutOfBounds();
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    void CheckForOutOfBounds()
    {
        if (transform.position.y < minimumHeight)
        {
            Respawn();
        }
    }

    void CheckForRespawn()
    {
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        //Debug.Log(charController.velocity);
        //charController.velocity.Set(0f,0f,0f);
        //charController.SimpleMove(Vector3.zero);
        //charController.Move(Vector3.zero);
        // Debug.Log("Respawning");

        /* charController.enabled = false;
         transform.position = respawnPoint;
         charController.enabled = true;

         currentHealth = maxHealth;

         Dru's working code*/

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Checkpoint"))
        {
            respawnPoint = transform.position;
        }

       /* if (collider.gameObject.tag == "OutOfBounds")
        {
            Respawn();
        }*/
    }

    void OnControllerColliderHit(ControllerColliderHit collision)
    {
       // print("yo");
        if (collision.gameObject.tag == "OutOfBounds")
        {
            Respawn();
        }
    }
}
