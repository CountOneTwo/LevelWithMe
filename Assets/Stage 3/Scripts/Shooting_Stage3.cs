using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_Stage3 : MonoBehaviour
{
    [Header("General")]
    public GameObject arrowPrefab;
    public GameObject projectileSpawn;
    public float coolDown;
    float coolDownTimer = 500;

    [Header("Windup")]
    public float windupMaximum;
    public float windupMinimum;
    float windup;

    [Header("Damage")]
    public float minDamage;
    public float maxDamage;

    [Header("Speed")]
    public float minSpeed;
    public float maxSpeed;

    [Header("Mass")]
    public float minMass;
    public float maxMass;

    [Header("Crosshair")]
    public GameObject leftHalf;
    public GameObject rightHalf;
    public GameObject fullDrawn;
    public float returnTime;
    float returnTimer;

    public int drawState = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer += Time.deltaTime;
        returnTimer += Time.deltaTime;
        if (!Regenaration_Stage3.regenerating && !HealthAndRespawn_Stage3.dead)
        {
            CheckForShooting();
        }
        

        CheckForCrosshairDisable();
    }
    public void CheckForCrosshairDisable()
    {
        if (returnTimer > returnTime)
        {
            DisableCrosshair();
        }
    }

    public void DisableCrosshair()
    {
        if (returnTimer < returnTime)
            returnTimer = returnTime + 1;

        leftHalf.SetActive(false);
        rightHalf.SetActive(false);
    }

    public void ReturnCombatCrosshair()
    {
        fullDrawn.SetActive(false);
        leftHalf.SetActive(true);
        rightHalf.SetActive(true);
        leftHalf.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 0);
        rightHalf.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);
    }

    void CheckForShooting()
    {
        if (coolDownTimer > coolDown)
        {
            if (drawState == 3)
            {
                drawState = 0;
            }
            if (Input.GetMouseButton(0) && !GameManager.gamePaused)
            {
                float windupPercentage = windup / windupMaximum;

                leftHalf.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50 + (33 * windupPercentage), 0);
                rightHalf.GetComponent<RectTransform>().anchoredPosition = new Vector2(50 - (33 * windupPercentage), 0);
                // leftHalf.transform.position = new Vector2(-50 + (33 * windupPercentage), 0);
                // rightHalf.transform.position = new Vector2(50 - (33 * windupPercentage), 0);


                windup += Time.deltaTime;
                returnTimer = 0;
                if (windup > windupMaximum)
                {
                    leftHalf.SetActive(false);
                    rightHalf.SetActive(false);
                    fullDrawn.SetActive(true);
                    windup = windupMaximum;
                    drawState = 2;
                }
                else
                {
                    drawState = 1;
                    leftHalf.SetActive(true);
                    rightHalf.SetActive(true);
                }
            }

            if (Input.GetMouseButtonUp(0) && !GameManager.gamePaused)
            {
                if (windup < windupMinimum)
                {
                    //windup = windupMinimum;
                    windup = 0;
                    return;
                }

                GameObject g = arrowPrefab;
                g.GetComponent<Arrow>().damage = minDamage + ((windup / windupMaximum) * (maxDamage - minDamage));
                g.GetComponent<Arrow>().speed = minSpeed + ((windup / windupMaximum) * (maxSpeed - minSpeed));
                g.GetComponent<Arrow>().mass = maxMass - ((windup / windupMaximum) * (maxMass - minMass));


                ReturnCombatCrosshair();

                Instantiate(g, projectileSpawn.transform.position, transform.rotation);
                drawState = 3;
                windup = 0;
                coolDownTimer = 0;
                returnTimer = 0;
            }
        }

    }
}
