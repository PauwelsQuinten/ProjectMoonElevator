using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _howToPlayUI;

    public void MoveToGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OpenHowToPlay()
    {
        _mainMenuUI.SetActive(false);
        _howToPlayUI.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        _mainMenuUI.SetActive(true);
        _howToPlayUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
