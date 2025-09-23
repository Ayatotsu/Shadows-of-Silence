using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject pausePanel;
    public void TogglePausePanel()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ToggleUnpausePanel()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    public void SwitchScene(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        Time.timeScale = 1f;
    }
}
