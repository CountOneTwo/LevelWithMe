using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndRespawn_Stage2 : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public Vector3 respawnPoint;
    public int currentHealth;
    [SerializeField] private float minimumHeight;
    public Slider healthBar;
    public static bool inCombat;
    public float disappearTime;
    float disappearTimer;
    private CharacterController charController;
    public GameObject indicator;

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
        healthBar.value = (float)currentHealth / (float)maxHealth;
        CheckForRespawn();
        CheckForOutOfBounds();
        CheckForDisappearance();
        disappearTimer += Time.deltaTime;
        if (healthBar.value == 1)
        {
            indicator.SetActive(false);
        }
        else
        {
            indicator.SetActive(true);
        }
    }


    void CheckForDisappearance()
    {
        if (disappearTimer > disappearTime && healthBar.gameObject.activeInHierarchy == true)
        {
            DeActivateHealthBar();
        }
    }

    void CheckForOutOfBounds()
    {
        if (transform.position.y < minimumHeight)
        {
            Respawn();
        }
    }

   public void ActivateHealthBar()
    {
        if (healthBar.gameObject.activeInHierarchy == false)
        {
            healthBar.gameObject.SetActive(true);
        }
        disappearTimer = 0;
    }

   public void DeActivateHealthBar()
    {
        healthBar.gameObject.SetActive(false);
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
        DeActivateHealthBar();
        charController.enabled = false;
        transform.position = respawnPoint;
        charController.enabled = true;

        currentHealth = maxHealth;
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
