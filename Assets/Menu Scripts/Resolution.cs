using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resolution : MonoBehaviour
{
    UnityEngine.Resolution[] resolution;
    List<string> resolutions;
    public Dropdown dropdown;
    public void Start()
    {
        resolutions = new List<string>();
        resolution = Screen.resolutions;
        foreach (var i in resolution)
        {
            resolutions.Add(i.width + "x" + i.height);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(resolutions);

    }
    public void resolutionChange(int r)
    {
        Screen.SetResolution(resolution[r].width, resolution[r].height, Screen.fullScreen);
    }
}
