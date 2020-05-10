﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Regenaration_Stage3 : MonoBehaviour
{
    public float fadeOutDuration;
    public float fadeInDuration;
    public float tickTime;
    public int healthPerTick;
    public float hideHealthBarTime;

    private Moving_Stage3 movementScript;
    private HealthAndRespawn_Stage3 healthScript;

    public static bool regenerating;
    bool currentlyFadingOut;

    public Image blurImage;
    public Image regenScreenEffect;

    float tickTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        movementScript = FindObjectOfType<Moving_Stage3>();
        healthScript = FindObjectOfType<HealthAndRespawn_Stage3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R) && movementScript.isGrounded && !currentlyFadingOut)
        {
            if (!regenerating)
            {
                StartCoroutine("FadeOutFunction", fadeOutDuration);
                regenerating = true;
            }
            else if (blurImage.color.a >= 1)
            {
                Regeneration();
            }
           

        }
        else
        {
            if (blurImage.color.a >= 1)
            {
                currentlyFadingOut = true;
                StartCoroutine("FadeInFunction", fadeInDuration);
            }
            
        }

    }

    void Regeneration()
    {
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickTime)
        {
            healthScript.currentHealth += healthPerTick;
            if (healthScript.currentHealth > healthScript.maxHealth)
            {
                healthScript.currentHealth = healthScript.maxHealth;
            }
        }
    }

    void DisableRegeneration()
    {
        regenerating = false;
        currentlyFadingOut = false;
    }

    IEnumerator FadeInFunction(float f)
    {
        while (blurImage.color.a > 0)
        {
            Color c = blurImage.color;
            c.a -= (Time.deltaTime * f);
            blurImage.color = c;
            yield return null;
        }
        DisableRegeneration();
    }

    IEnumerator FadeOutFunction(float f)
    {
        while (blurImage.color.a < 1)
        {

            Color c = blurImage.color;
            c.a += (f * Time.deltaTime);
            blurImage.color = c;
            yield return null;
        }
    }
}