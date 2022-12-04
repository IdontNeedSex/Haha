using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenesScript : MonoBehaviour
{
    public void SwitchSceneToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void CloseApplication()
    {
        Application.Quit();
    }
}
