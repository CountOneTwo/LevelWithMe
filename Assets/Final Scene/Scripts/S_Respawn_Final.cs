using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class S_Respawn_Final : MonoBehaviour
{
    Vector3 respawnPoint;
    [SerializeField] private float minimumHeight;

    private CharacterController charController;


    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForOutOfBounds();
    }

    void CheckForOutOfBounds()
    {
        if (transform.position.y < minimumHeight)
        {
            Respawn();
        }
    }

    void Respawn()
    {
         charController.enabled = false;
         transform.position = respawnPoint;
         charController.enabled = true;
    }

}
