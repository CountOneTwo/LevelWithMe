using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool devNoteCurrentlyDisplayed = false;
    public static bool gamePaused = false;
    public static InformationIconV2 currentDevNote;
    public Canvas pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenu.enabled = false;
        gamePaused = false;

        Time.timeScale = 1;     
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        //Unpause Game or Pause Game
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

        if(pauseMenu.GetComponent<S_PauseMenu_Stage3>() != null)
        {
            pauseMenu.GetComponent<S_PauseMenu_Stage3>().PlayToggleSound();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
