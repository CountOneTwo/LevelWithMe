using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    Image blackScreen;
    public float fadeinFactor;
    // Start is called before the first frame update
    void Start()
    {
        blackScreen = GetComponent<Image>();
        StartCoroutine("FadeInFunction");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeInFunction()
    {
        while (blackScreen.color.a > 0)
        {
            Color c = blackScreen.color;
            c.a -= (Time.deltaTime / fadeinFactor);
            blackScreen.color = c;
            yield return null;
        }
    }
}
