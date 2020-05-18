using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoalReached : MonoBehaviour
{
    public float fadeoutFactor;
    public Image blackScreen;
    // Start is called before the first frame update

    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Goal")
        {
            blackScreen.enabled = true;
            StartCoroutine("FadeOutFunction");
        }
    }

    void LevelTransition()
    {
        //Load next level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Increases the alpha of the blackscreen until the screen is fully black --> triggers level transition
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
