using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateHealthAndShooting_Stage2 : MonoBehaviour
{
    public Shooting_Stage2 shooting;
    public Text healthCounter;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Player"))
        {
            shooting.enabled = true;
            healthCounter.enabled = true;
        }
    }
}
