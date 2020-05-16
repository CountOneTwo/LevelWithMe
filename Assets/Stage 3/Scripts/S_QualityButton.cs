using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_QualityButton : MonoBehaviour
{
    public Button buttonRef;
    public S_PauseMenu_Stage3 pauseMenu;
    public Text label;
    public int qualityIndex = 0;

    private void Start()
    {
        if (label != null)
        {
            label.text = QualitySettings.names[qualityIndex];
        }
    }

    public void OnClick()
    {
        if (pauseMenu != null)
        {
            pauseMenu.OnQualityButtonPressed(this);
        }
    }

    public void SetEnabled(bool enabled)
    {
        if (buttonRef != null)
        {
            buttonRef.interactable = enabled;
        }
    }
   
    
}
