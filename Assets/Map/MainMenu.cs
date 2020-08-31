using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void SceneLoader()
    {
        SceneManager.LoadScene("SampleScene");
    }
   public void ExitApproach()
    {
        Application.Quit();
        Debug.Log("Exited");
    }
}
