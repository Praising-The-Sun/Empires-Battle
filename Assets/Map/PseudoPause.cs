using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class PseudoPause : MonoBehaviour
{
    public bool isPaused = false;
    [Header("Canvas")]
    [SerializeField]
    public Canvas canvas;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            canvas.gameObject.SetActive(isPaused);
            Cursor.visible = isPaused;
        }
    }
    public void ButtonReturn()
    {
        isPaused = false;
        canvas.gameObject.SetActive(isPaused);
        Cursor.visible = isPaused;
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}



    


