using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationIcon : MonoBehaviour
{
    bool thisNoteDisplayed;

    Renderer renderer;

    float timeToDisplay;

    float timeLookedAt;

    float showArea;
    float hideArea;

    public Canvas developerComment;
    public Material whenRead;
    public Slider progressSlider;
    DeveloperNote parentNode;

    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        parentNode = GetComponentInParent<DeveloperNote>();
        timeToDisplay = parentNode.timeToDisplay;
        showArea = parentNode.showArea;
        hideArea = parentNode.hideArea;

    }

    // Update is called once per frame
    void Update()
    {
        VisibilityCheck();
        transform.LookAt(new Vector3(GameObject.Find("Player").transform.position.x, transform.position.y, GameObject.Find("Player").transform.position.z));
    }

    void VisibilityCheck()
    {
        if (GameManager.devNoteCurrentlyDisplayed == false)
        {
            if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < showArea)
            {

                if (renderer.isVisible)
                {
                    Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                    RaycastHit hit;



                    if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                    {
                        timeLookedAt += Time.deltaTime;
                        if (timeLookedAt > timeToDisplay)
                        {

                            developerComment.enabled = true;
                            renderer.enabled = false;
                            renderer.material = whenRead;
                            progressSlider.gameObject.SetActive(false);

                            developerComment.gameObject.transform.LookAt(new Vector3(GameObject.Find("Player").transform.position.x, developerComment.gameObject.transform.position.y, GameObject.Find("Player").transform.position.z));

                            GameManager.devNoteCurrentlyDisplayed = true;
                            thisNoteDisplayed = true;
                            print("Eyo");

                            foreach (Renderer r in GetComponentsInChildren<Renderer>())
                                r.enabled = false;

                        }
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
            }
        }
        else if (thisNoteDisplayed == true)
        {
            bool currentlyLookedAt = false;
            foreach (Renderer r in developerComment.gameObject.GetComponentsInChildren<Renderer>())
            {
                if (r.isVisible)
                {
                    //print("EYYESS");
                    currentlyLookedAt = true;
                }
            }
               



            //Check if is visible
            if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) > hideArea || !currentlyLookedAt)
            {
                DisableDevNote();
                timeLookedAt = 0;
            }
        }

        progressSlider.value = timeLookedAt / timeToDisplay;

    }


    void DisableDevNote()
    {
        print("EyNOO");
        GameManager.devNoteCurrentlyDisplayed = false;
        thisNoteDisplayed = false;
        developerComment.enabled = false;
        progressSlider.gameObject.SetActive(true);
        GetComponent<Renderer>().enabled = true;
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = true;

    }
    
}
