using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BackToMain : MonoBehaviour
{
    public void BackOnClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
