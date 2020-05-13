using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndRespawn_Stage2 : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    
    [HideInInspector]
    public Vector3 respawnPoint;
    [HideInInspector]
    public Vector3 respawnOrientation;


    public int currentHealth;
    [SerializeField] private float minimumHeight;
    public Slider healthBar;
    public static bool inCombat;
    public float disappearTime;
    float disappearTimer;
    private CharacterController charController;
    public GameObject indicator;
    GameObject mainCamera;

    public float fadeDuration;

    public static bool dead;

    [Header("Death Sound")]
    public AudioSource DeathSource;
    public AudioClip[] DeathClips;

    public bool PitchChangeEnabledDeath;
    [Range(-3, 3)] public float CurrentPitchDeath;
    [Range(-3, 3)] public float MinPitchDeath;
    [Range(-3, 3)] public float MaxPitchDeath;
    public bool VolumeChangeEnabledDeath;
    [Range(0, 1)] public float CurrentVolumeDeath;
    [Range(0, 1)] public float MinVolumeDeath;
    [Range(0, 1)] public float MaxVolumeDeath;

    [Header("Hit Sound")]
    public AudioSource HitSource;
    public AudioClip[] HitClips;

    public bool PitchChangeEnabledHit;
    [Range(-3, 3)] public float CurrentPitchHit;
    [Range(-3, 3)] public float MinPitchHit;
    [Range(-3, 3)] public float MaxPitchHit;
    public bool VolumeChangeEnabledHit;
    [Range(0, 1)] public float CurrentVolumeHit;
    [Range(0, 1)] public float MinVolumeHit;
    [Range(0, 1)] public float MaxVolumeHit;

    private float healthLastFrame;
    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
        respawnPoint = transform.position;
        respawnOrientation = transform.eulerAngles;
        charController = GetComponent<CharacterController>();

    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthLastFrame = maxHealth;

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
        if (currentHealth < healthLastFrame)
        {
            HitSound();
        }
        healthLastFrame = currentHealth;
        //print(respawnPoint);
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
        disappearTimer = 0;
    }

   public void DeActivateHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }



    void CheckForRespawn()
    {
        if (currentHealth <= 0 && !dead)
        {
           PrepareRespawn();
        }
    }

    void PrepareRespawn()
    {
        dead = true;
        DeathSound();
        GameObject.Find("Blackscreen").GetComponent<Fade_Stage23>().RespawnFadeOut(fadeDuration);
        
        
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
        GetComponent<Moving_Stage2>().cooldown = false;
        GetComponentInChildren<Shooting_Stage2>().DisableCrosshair();
        charController.enabled = false;
        transform.position = respawnPoint;
        transform.eulerAngles = respawnOrientation;
        charController.enabled = true;
        mainCamera.transform.localEulerAngles = Vector3.zero;
        currentHealth = maxHealth;
    }

    void OnTriggerEnter(Collider collider)
    {
       /* if (collider.gameObject.name.Equals("Checkpoint"))
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
        if (collision.gameObject.tag == "OutOfBounds"  && !dead)
        {
            PrepareRespawn();
        }
    }

    void DeathSound()
    {
        if (PitchChangeEnabledDeath == true)
        {
            CurrentPitchDeath = Random.Range(MinPitchDeath, MaxPitchDeath);
        }
        if (VolumeChangeEnabledDeath == true)
        {
            CurrentVolumeDeath = Random.Range(MinVolumeDeath, MaxVolumeDeath);
        }
        DeathSource.clip = DeathClips[Random.Range(0, DeathClips.Length)];
        DeathSource.pitch = CurrentPitchDeath;
        DeathSource.volume = CurrentVolumeDeath;
        DeathSource.Play();

    }
    void HitSound()
    {
        if (PitchChangeEnabledHit == true)
        {
            CurrentPitchHit = Random.Range(MinPitchHit, MaxPitchHit);
        }
        if (VolumeChangeEnabledHit == true)
        {
            CurrentVolumeHit = Random.Range(MinVolumeHit, MaxVolumeHit);
        }
        HitSource.clip = HitClips[Random.Range(0, HitClips.Length)];
        HitSource.pitch = CurrentPitchHit;
        HitSource.volume = CurrentVolumeHit;
        HitSource.Play();
    }
}
