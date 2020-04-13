using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndRespawn : MonoBehaviour
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
        healthCounter.text = "Health: " + currentHealth;
        CheckForRespawn();
        CheckForOutOfBounds();
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
        transform.position = respawnPoint;
        currentHealth = maxHealth;
    }
}
