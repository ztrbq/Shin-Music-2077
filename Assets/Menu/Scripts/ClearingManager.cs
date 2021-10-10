using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearingManager : MonoBehaviour
{
    [SerializeField]
    Text perfect = null, good = null, miss = null, score = null;

    private void Start()
    {
        perfect.text = Score.Score.perfectNum.ToString();
        good.text = Score.Score.goodNum.ToString();
        miss.text = Score.Score.missNum.ToString();
        score.text = Score.Score.totalScore.ToString();
    }

    public void backBtnOnClick()
    {        
        SceneManager.LoadScene("Menu");
    }
    public void selectOtherBtnOnClick()
    {
        SceneManager.LoadScene("SelectSongMenu");
    }
    public void restartBtnOnClick()
    {
        SceneManager.LoadScene("Main");
    }
}
