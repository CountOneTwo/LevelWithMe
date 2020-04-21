using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalReached : MonoBehaviour
{
    public float fadeoutFactor;
    public Image blackScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Goal")
        {
            StartCoroutine("FadeOutFunction");
        }
    }

    void LevelTransition()
    {
        //Load next level
    }

    IEnumerator FadeOutFunction()
    {
        while (blackScreen.color.a < 1)
        {
            Color c = blackScreen.color;
            c.a += (Time.deltaTime / fadeoutFactor);
            blackScreen.color = c;
            yield return null;
        }
        LevelTransition();
    }


}
