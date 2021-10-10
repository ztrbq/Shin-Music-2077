using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGameButtonOnclick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void quitBtnOnClick()
    {
        Application.Quit();
    }
}
