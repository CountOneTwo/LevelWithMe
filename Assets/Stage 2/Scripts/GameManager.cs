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
        pauseMenu.enabled = false;
        gamePaused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
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
