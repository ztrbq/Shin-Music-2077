using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonBehave : MonoBehaviour
{
    public void PauseOnClick(GameObject canvas)
    {
        Time.timeScale = 0;
        Background.Background.PlanesActivity = true;
        GameObject musicPlayer = GameObject.Find("MusicPlayer");
        AudioSource audioSource = (AudioSource)musicPlayer.GetComponent("AudioSource");
        audioSource.Pause();
        canvas.SetActive(true);
    }
}
