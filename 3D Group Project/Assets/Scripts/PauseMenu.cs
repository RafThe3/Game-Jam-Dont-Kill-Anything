using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas pauseMenu;
    [SerializeField] private CharacterController player;

    private void Start()
    {
        pauseMenu.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnablePauseMenu();
        }
    }

    public void EnablePauseMenu()
    {
        switch (Time.timeScale)
        {
            case 1:
                Time.timeScale = 0;
                pauseMenu.enabled = true;
                break;
            default:
                Time.timeScale = 1;
                pauseMenu.enabled = false;
                break;
        }
    }

    public void Resume()
    {
        pauseMenu.enabled = false;
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
