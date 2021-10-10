using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSongManager : MonoBehaviour
{
    public void SelectButtonOnClick(string sceneName)
    {
        //Application.LoadLevel(sceneName);
        SceneManager.LoadScene("sceneName");
    }
    public void HardButtonOnClick()
    {
        Settings.Settings.difficulty = Settings.Difficulty.difficult;
    }
    public void EasyButtonOnClick()
    {
        Settings.Settings.difficulty = Settings.Difficulty.easy;
    }
    public void backBtnOnClick()
    {
        //Application.LoadLevel("Menu");
        SceneManager.LoadScene("Menu");
    }

    public void imageButton(string songName)
    {
        Settings.Settings.ChosenSong = songName;
    }

    public void startBtnOnClick()
    {
        switch (Screen.width)
        {
            case 2160: 
                Settings.Settings.edgePos = -0.66f;
                Settings.Settings.SurfacePos = 0.8f;
                break;
            case 1920:
                Settings.Settings.edgePos = -0.12f;
                Settings.Settings.SurfacePos = 1.5f;
                break;
            default: 
                break;
        }
        Debug.Log("变色面在："+ Settings.Settings.SurfacePos);
        //Application.LoadLevel("Main");
        SceneManager.LoadScene("Main");


    }

}
