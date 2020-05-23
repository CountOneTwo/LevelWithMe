using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Stage3 : MonoBehaviour
{
    public static bool devNoteCurrentlyDisplayed = false;
    public static bool gamePaused = false;
    public static InformationIcon currentDevNote;
    public Canvas pauseMenu;

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
        //Unpause or pause game
        if (gamePaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            pauseMenu.enabled = false;
            gamePaused = false;

            Time.timeScale = 1;
                      
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;            

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
