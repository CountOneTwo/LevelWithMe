using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationIcon : MonoBehaviour
{
    Renderer renderer;
    public float timeToDisplay;
    float timeLookedAt;
    public float range;
    public Canvas developerComment;
    public Material whenRead;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        VisibilityCheck();
    }

    void VisibilityCheck()
    {
        if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < range)
        {
            if (renderer.isVisible)
            {
                timeLookedAt += Time.deltaTime;
                if (timeLookedAt > timeToDisplay)
                {
                    //Display comment
                    developerComment.enabled = true;
                    renderer.enabled = false;
                    renderer.material = whenRead;
                }
            }
            else
            {
                timeLookedAt = 0;
            }

        }
        else
        {
       
            developerComment.enabled = false;
            renderer.enabled = true;
            timeLookedAt = 0;
        }
           /* if (renderer.isVisible)
        {
            if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < range)
            {
                timeLookedAt += Time.deltaTime;
                if (timeLookedAt > timeToDisplay)
                {
                    //Display comment
                    developerComment.enabled = true;
                    renderer.enabled = false;
                }
            }
            else
            {
                timeLookedAt = 0;
            }
        }
        else
        {
            timeLookedAt = 0;
        }*/
    }
    
}
