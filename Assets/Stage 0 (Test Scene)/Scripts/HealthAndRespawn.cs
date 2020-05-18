using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndRespawn : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float minimumHeight;

    public Vector3 respawnPoint;
    public Text healthCounter;

    [HideInInspector]
    public int currentHealth;
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
        healthCounter.text = "Health: " + currentHealth;

        CheckForRespawn();

        CheckForOutOfBounds();
    }

    //Checks if player is below the level
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

    //Move the player to the spawn and reset his health
    void Respawn()
    {
        charController.enabled = false;
        transform.position = respawnPoint;
        charController.enabled = true;
        currentHealth = maxHealth;
    }
}
