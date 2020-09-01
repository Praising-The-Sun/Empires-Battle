using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFullscreen : MonoBehaviour
{
    public bool isFullscreen = true; 
    public void FullScreenToggle()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
        Debug.Log("Screen mode changed");
    }
}

