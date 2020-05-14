﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndRespawn_Stage3 : MonoBehaviour
{
    public int maxHealth;

    [HideInInspector]
    public Vector3 respawnPoint;
    [HideInInspector]
    public Vector3 respawnOrientation;
    [HideInInspector]
    public int currentHealth;
    [SerializeField] private float minimumHeight;
    public Slider healthBar;
    public static bool inCombat;
    public float disappearTime;
    [HideInInspector]
    public float disappearTimer;
    private CharacterController charController;
    public GameObject indicator;
    public GameObject lowHealthScreenEffect;
    public static bool dead;
    GameObject mainCamera;

    private S_PlayerStreaming playerStreaming;
    private GameObject[] LoadOnRepawn;
    private GameObject[] UnloadOnRespawn;

    public float fadeDuration;

    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
        respawnPoint = transform.position;
        respawnOrientation = transform.eulerAngles;
        charController = GetComponent<CharacterController>();
        playerStreaming = GetComponent<S_PlayerStreaming>();
        
        if (playerStreaming != null)
        {
            LoadOnRepawn = playerStreaming.ObjectsToLoadOnStart;
            UnloadOnRespawn = playerStreaming.ObjectsToUnloadOnStart;
        }
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
            lowHealthScreenEffect.SetActive(false);
        }
        else if(healthBar.value <= 0.3)
        {
            lowHealthScreenEffect.SetActive(true);


            indicator.SetActive(true);
        }
        else
        {
            lowHealthScreenEffect.SetActive(false);

            indicator.SetActive(true);
        }
    }

    public void UpdateObjectsToLoadOnRespawn(GameObject[] newUnload, GameObject[] newLoad)
    {
        UnloadOnRespawn = newUnload;
        LoadOnRepawn = newLoad;
    }

    void CheckForDisappearance()
    {
        if (disappearTimer > disappearTime && healthBar.gameObject.activeInHierarchy == true && Regenaration_Stage3.healthBarTimer <= 0)
        {
            DeActivateHealthBar();
        }
    }

    void CheckForOutOfBounds()
    {
        if (transform.position.y < minimumHeight && !dead)
        {
            PrepareRespawn();
        }
    }

    public void ActivateHealthBar()
    {
        if (healthBar.gameObject.activeInHierarchy == false)
        {
            healthBar.gameObject.SetActive(true);
        }
        //disappearTimer = 0;
    }

    public void DeActivateHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }

    void PrepareRespawn()
    {
        dead = true;
        GameObject.Find("Blackscreen").GetComponent<Fade_Stage23>().RespawnFadeOut(fadeDuration);

    }

    void CheckForRespawn()
    {
        if (currentHealth <= 0 && !dead)
        {
            PrepareRespawn();
        }
    }

    public void Respawn()
    {
        //Debug.Log(charController.velocity);
        //charController.velocity.Set(0f,0f,0f);
        //charController.SimpleMove(Vector3.zero);
        //charController.Move(Vector3.zero);
        // Debug.Log("Respawning");
        dead = false;
        DeActivateHealthBar();
        GetComponent<Moving_Stage3>().cooldown = false;

        //print(respawnPoint);
        GetComponentInChildren<Shooting_Stage3>().DisableCrosshair();
        charController.enabled = false;
        transform.position = respawnPoint;
        transform.eulerAngles = respawnOrientation;

        playerStreaming.LoadObjects(UnloadOnRespawn, false);
        playerStreaming.LoadObjects(LoadOnRepawn, true);

        charController.enabled = true;
        mainCamera.transform.localEulerAngles = Vector3.zero;
        currentHealth = maxHealth;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Checkpoint"))
        {
            respawnPoint = transform.position;
        }

    }

    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        // print("yo");
        if (collision.gameObject.tag == "OutOfBounds" && !dead)
        {
            PrepareRespawn();
        }
    }
}
