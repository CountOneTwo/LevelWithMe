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
    public Slider progressSlider;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        VisibilityCheck();
        transform.LookAt(new Vector3(GameObject.Find("Player").transform.position.x, transform.position.y, GameObject.Find("Player").transform.position.z));
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
                    progressSlider.gameObject.SetActive(false);

                    developerComment.gameObject.transform.LookAt(new Vector3(GameObject.Find("Player").transform.position.x, developerComment.gameObject.transform.position.y, GameObject.Find("Player").transform.position.z));


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
            progressSlider.gameObject.SetActive(true);
            renderer.enabled = true;
            timeLookedAt = 0;
        }

        progressSlider.value = timeLookedAt / timeToDisplay;
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
