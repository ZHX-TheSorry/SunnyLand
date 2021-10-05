using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioMixer audioMixer;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UIEnable()
    {
        GameObject.Find("Canvas/menu/UI").SetActive(true);  //查找文件并激活
    }


    public void PauseGame()
    {
        pauseMenu.SetActive(true);      //激活pauseMenu
        Time.timeScale = 0f;
    }


    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }


    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }


    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MainVolume",value);
    }
}
