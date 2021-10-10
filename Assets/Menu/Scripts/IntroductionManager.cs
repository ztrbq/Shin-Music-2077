using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductionManager : MonoBehaviour
{
    public void IntroductionOnClick(string sceneName)
    {
        //Application.LoadLevel(sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
