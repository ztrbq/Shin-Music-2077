using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutUsManager : MonoBehaviour
{
    public void AboutUsOnClick(string sceneName)
    {
        //Application.LoadLevel(sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
