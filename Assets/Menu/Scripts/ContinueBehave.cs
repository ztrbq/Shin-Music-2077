using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueBehave : MonoBehaviour
{
    CountDown count;

    private void Start()
    {
        count = GameObject.Find("MusicPlayer").GetComponent<CountDown>();    
    }

    public void ContinueOnClick(GameObject canvas)
    {
        canvas.SetActive(false);
        StartCoroutine(count.Count());
        //在这里调用321动画,TimeScale的修改在321动画脚本里面,321脚本里面还要继续播放音乐
    }
}
