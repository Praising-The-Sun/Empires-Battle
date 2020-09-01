using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSettings : MonoBehaviour
{
    public bool settingsToggled = false;
    public Canvas canvas;
    public void Toggle()
    {
        settingsToggled = !settingsToggled;
        canvas.gameObject.SetActive(settingsToggled);
    }
}
