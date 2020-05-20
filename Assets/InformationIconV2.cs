using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationIconV2 : MonoBehaviour
{
    bool thisNoteDisplayed;

    Image unreadIcon;

    float timeToDisplay;

    float timeLookedAt;

    float showArea;
    float hideArea;

    public Canvas developerComment;

    public GameObject parentCanvas;

    public Image progressSlider;
    public Image readIcon;

    S_DevNote_Final parentNode;

    Camera mainCamera;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        unreadIcon = GetComponent<Image>();
        parentNode = GetComponentInParent<S_DevNote_Final>();
        timeToDisplay = parentNode.timeToDisplay;
        showArea = parentNode.showArea;
        hideArea = parentNode.hideArea;

    }

    // Update is called once per frame
    void Update()
    {
        VisibilityCheck();
        parentCanvas.transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }

    void VisibilityCheck()
    {
        if (!thisNoteDisplayed)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < showArea)
            {


                    Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                    RaycastHit hit;



                    if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                    {
                        timeLookedAt += Time.deltaTime;
                        if (timeLookedAt > timeToDisplay)
                        {

                            EnableDevNote();

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
        else
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
            if (Vector3.Distance(transform.position, player.transform.position) > hideArea || !currentlyLookedAt)
            {
                DisableDevNote();
                timeLookedAt = 0;
            }
        }

        progressSlider.fillAmount = timeLookedAt / timeToDisplay;

    }

    void EnableDevNote()
    {
        if (GameManager.devNoteCurrentlyDisplayed)
        {
            GameManager.currentDevNote.DisableDevNote();

        }

        GameManager.currentDevNote = this;

        developerComment.enabled = true;
        unreadIcon.enabled = false;
        readIcon.enabled = false;
        progressSlider.gameObject.SetActive(false);

        developerComment.gameObject.transform.LookAt(new Vector3(player.transform.position.x, developerComment.gameObject.transform.position.y, player.transform.position.z));

        GameManager.devNoteCurrentlyDisplayed = true;
        thisNoteDisplayed = true;
        //print("Eyo");

    }


    void DisableDevNote()
    {
        // print("EyNOO");
        GameManager.devNoteCurrentlyDisplayed = false;
        thisNoteDisplayed = false;
        developerComment.enabled = false;
        progressSlider.gameObject.SetActive(true);
        readIcon.enabled = true;

    }

}
