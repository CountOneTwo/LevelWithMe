using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade_Stage23 : MonoBehaviour
{
    Image blackScreen;
    public float fadeinFactor;
    // Start is called before the first frame update
    void Start()
    {
        blackScreen = GetComponent<Image>();
        StartCoroutine("FadeInFunction", fadeinFactor);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RespawnFadeIn(float f)
    {
        StartCoroutine("FadeInFunction", f);
    }

    public void RespawnFadeOut(float f)
    {
        StopAllCoroutines();
        StartCoroutine("FadeOutFunction", f);

    }

    //Fade in --> Decreases the alpha each frame until the blackscreen is gone
    IEnumerator FadeInFunction(float f)
    {
        while (blackScreen.color.a > 0)
        {
            Color c = blackScreen.color;
            c.a -= (Time.deltaTime * f);
            blackScreen.color = c;
            yield return null;
        }
    }

    //Fade out --> Increases the alpha each frame until the screen is black --> Triggers Respawn at complete fadout and starts fade in again
    IEnumerator FadeOutFunction(float f)
    {
        while (blackScreen.color.a < 1)
        {

            Color c = blackScreen.color;
            c.a += (f * Time.deltaTime);
            blackScreen.color = c;
            yield return null;
        }

        if (GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage2>() != null && GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage2>().enabled)
        {
            GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage2>().Respawn();
        }
        else if (GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage3>() != null && GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage3>().enabled)
        {
            GameObject.Find("Player").GetComponent<HealthAndRespawn_Stage3>().Respawn();
        }

        RespawnFadeIn(f);

        
    }
}
