using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Stage3 : MonoBehaviour
{
    public static bool devNoteCurrentlyDisplayed = false;
    public static bool gamePaused = false;
    public static InformationIcon currentDevNote;
    public Canvas pauseMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Regenaration_Stage3.regenerating)
        {
            if (Input.GetButtonDown("Pause"))
            {
                TogglePause();
            }
        }


    }

    public void TogglePause()
    {
        if (gamePaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.enabled = false;
            gamePaused = false;
            Time.timeScale = 1;
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.enabled = true;
            gamePaused = true;
            Time.timeScale = 0;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
