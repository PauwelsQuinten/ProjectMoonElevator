using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUi : MonoBehaviour
{
    private void Start()
    {
        
    }
    public void LoadNewGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
