using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator doorOpenAnimator;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = true;
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void GameScene(string packId, string levelId)
    {
        LevelManager.CurrentLevelId = levelId;
        LevelManager.CurrentPackId = packId;
        if (doorOpenAnimator)
        {
            doorOpenAnimator.Play("DoorOpenAnimation");
        }
        Invoke("EnterGame", 1);
    }

    void EnterGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
