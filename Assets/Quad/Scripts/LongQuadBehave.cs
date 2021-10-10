using Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Settings;
using UnityEngine.UI;

class LongQuadBehave : QuadBehave
{ 
    private Note_NoteStrip currentNoteStrip=new Note_NoteStrip(10,2,5);
    bool isMiss = true;  //如果音符条的头部出了按键区域后都没有按下去，那么就miss   初始为true是采用“反看”的思想，因为先假设miss，当不miss时改为false，否则就不改，这样在代码上好实现
    //use this to init
    public void Initialize(Note.Note_NoteStrip note)
    {
        m_note = note;
        currentNoteStrip = note;
    }

    private new void Start()
    {
        m_quad = this.gameObject;
        Background.Background.SetPositionAtTrack(transform, m_note.trackNum, this.transform.localScale.z / 2);
        m_nowPos = transform.position;
        QuadMaterial.OnLeave(this.gameObject);
    }

    private new void FixedUpdate()
    {
        m_nowPos.Set(m_nowPos.x, m_nowPos.y, m_nowPos.z - m_vel * Time.deltaTime);
        this.transform.position = m_nowPos;
        if (CheckHit())
        {
            Debug.Log("真的点中了");
            QuadMaterial.OnTouch(this.gameObject);
        }
        if (CheckOut())
        {
            QuadPool.Die(this.gameObject);
        }

    }
    //这个函数根据“玩家触摸的位置”“音符条的位置”“指示音符条是否有效的变量IsValid”来决定返回值
    //这个函数还会根据“玩家触摸的位置”“音符条的位置”维护IsValid变量
    override public bool CheckHit()
    {        
        if (this.IsValid == false) //如果这个音符条失效了，那么玩家就不能再触发音符了，所以就没必要检测是否成功触发音符了，即我就认为没有hit到音符，即返回false       
            return false;
        //如果音符条有效，那么首先要：获取玩家触摸位置和音符条的位置，然后根据（“玩家触摸位置到位”且“音符条到位”）与否做不同的事       
        float zOfHeadSide = m_nowPos.z - m_quad.transform.localScale.z / 2; //zOfHeadSide是音符条靠近按键的一边的中点的z坐标
        if (zOfHeadSide >= Settings.Settings.SurfacePos) //如果这个音符条的头压根还没到变色面，那么按键不按键都是没有hit到
            return false;
        //音符条的头已经过了变色面，即音符条的一部分变色了
        float zOfTailSide = m_nowPos.z + m_quad.transform.localScale.z / 2; //zOfTailSide是音符条远离按键的一边的中点的z坐标
        if (zOfTailSide <= Settings.Settings.edgePos) //音符条的尾边出了屏幕，既然整个音符条都出了屏幕，那么按键不按键都是没有hit
        {
            if (!isMiss && IsValid)
                Score.Score.perfectNum += 1;
            IsValid = false;
            return false;
        }
        else if (zOfHeadSide >= Settings.Settings.edgePos) //音符条的尾边没有出屏幕 且 音符条的首边没有出屏幕
        {
            bool ifHaveTheTouch = haveTouchOfTheTrack(this.m_note.trackNum);
            Debug.Log("ifHaveTheTouch为"+ifHaveTheTouch);
            //Debug.Log("这个音符条的通道为"+ this.m_note.trackNum);
            if (ifHaveTheTouch)
            {
                Debug.Log("加分");
                Score.Score.totalScore += 0.02f * 10.0f * MusicScore.MusicScoreManager.musicScore.scorePerSecOfNoteStrip;
                isMiss = false;  //表示没有miss掉
                return true;
            }             
            //返回false是因为没有hit，不修改isValid和isMiss是因为它还有机会被按下触发
            return false;
            
        }
        else////音符条的尾边没有出屏幕 且 音符条的首边出了屏幕
        {
            if (isMiss)//如果miss掉了音符条，那么音符条失效，同时也是没有hit到
            {
                //Debug.Log("音符条miss");
                IsValid = false;
                Score.Score.missNum += 1;
                return false;

            }
            else//没有miss掉音符条的头
            {
                bool ifHaveTheTouch = haveTouchOfTheTrack(this.m_note.trackNum);
                if (ifHaveTheTouch)
                {
                    Debug.Log("加分");
                    Score.Score.totalScore += 0.02f * 10.0f * MusicScore.MusicScoreManager.musicScore.scorePerSecOfNoteStrip;
                    return true;
                }
                else
                {
                    QuadMaterial.OnLeave(gameObject);
                    IsValid = false;
                    Score.Score.goodNum += 1;
                    return false;
                }
            }
        }








    }
    override public bool CheckOut()
    {
        if (m_nowPos.z + currentNoteStrip.lastTime / 2 == 0/* 最后的边界变为了0 */)
            return true;
        return false;
    }

}

