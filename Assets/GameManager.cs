﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool devNoteCurrentlyDisplayed = false;
    public static bool gamePaused = false;
    public Canvas pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }

    }

    void TogglePause()
    {
        if (gamePaused)
        {
            pauseMenu.enabled = false;
            gamePaused = false;
            Time.timeScale = 1;
        }
        else
        {
            pauseMenu.enabled = true;
            gamePaused = true;
            Time.timeScale = 0;
        }
    }
}
