using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Score;

public class ShortQuadBehave : QuadBehave
{

    //use this to init
    public void Initialize(Note.Note_NoteBar note)
    {
        m_note = note;
    }


    /*
    override public bool CheckHit()
    {
        if (this.IsValid == false) //如果这个音符条失效了，那么玩家就不能再触发音符了，所以就没必要检测是否成功触发音符了，即我就认为没有hit到音符，即返回false       
            return false;
        float scoreLevel = 0.0f;
        float sForBar = MusicScore.MusicScoreManager.musicScore.scorePerSecOfNoteStrip;
        float nowPosition = this.m_nowPos.z;
        //Debug.Log(nowPosition);
        if (nowPosition <= GoodLeft && nowPosition>=GoodRight && haveTouchOfTheTrack(this.m_note.trackNum))
        {
            Debug.Log("是否被按下" + haveTouchOfTheTrack(this.m_note.trackNum));
            Debug.Log("good");
            scoreLevel = 0.5f;//Good

            Score.Score.totalScore += scoreLevel * sForBar;
            Score.Score.goodNum += 1;
            this.IsValid = false;//触摸一次之后按键失效处理
            return true;
        }
        return false;
    }*/

    // /*
    //todo:检测触摸判断
    override public bool CheckHit()
    {
        if (this.IsValid == false) return false;//按键若失效 直接失败

        float borderLine = 0;//合法检测区
        float scoreLevel = 0; //表示得分的等级 0.5 - good     1 - perfect      0 - miss
        float sForBar= MusicScore.MusicScoreManager.musicScore.scorePerSecOfNoteStrip;
        switch (Screen.width)
        {
            case 2160: borderLine = 0.7f; break;
            case 1920: borderLine = 0.5f; break;
            default:
                borderLine = 1.0f;
                break;
        }
        //Vector3 CameraPos = new Vector3(0, 0, -5);//摄像机的世界坐标
        //Vector3 TouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray[] allTouchRay = this.getAllRaysThroughTouches();


        foreach (Ray ray in allTouchRay)
        {
            if (this.IsValid == false) return false;//按键若失效 直接失败

            RaycastHit result;
            int layerMask = 1<<8;
            if (Physics.Raycast(ray, out result,100, layerMask))
            {
                //Debug.Log("hitttttttttttttttttttttttttttttttttttttttttttt");
                //Debug.Log(GoodRight+":" +GoodLeft+":" + PerfectLeft+":" + PerfectRight);
                //Debug.Log("现在的位置是"+(this.m_nowPos.z - this.m_quad.transform.localScale.z / 2));
                float distance = this.m_nowPos.z - this.m_quad.transform.localScale.z / 2;
                //float distance = this.m_nowPos.z;//获取按键的y坐标
                //float distance = result.point.z;
                //Debug.Log("触摸点的距离是"+distance);
                if (distance > 4 * borderLine) return false;//在合法触摸区之外 直接返回

                if (distance < GoodRight && distance > PerfectRight)
                {
                    //Debug.Log("good");
                    scoreLevel = 0.5f;//Good
                    Score.Score.totalScore += scoreLevel * sForBar;
                    Score.Score.goodNum += 1;
                    GetComponent<Breakable>().PieceUp();//碎片特效
                    GetComponent<ParticleSystem>().Play();//粒子特效播放
                    QuadPool.Die(gameObject);
                    this.IsValid = false;//触摸一次之后按键失效处理

                    return true;
                }
                else if (distance > PerfectLeft)
                {
                    //Debug.Log("perfect");

                    scoreLevel = 1f;//perfect 得分
                    Score.Score.totalScore += scoreLevel * sForBar;
                    Score.Score.perfectNum += 1;
                    GetComponent<Breakable>().PieceUp();
                    GetComponent<ParticleSystem>().Play();//粒子特效播放
                    QuadPool.Die(gameObject);
                    this.IsValid = false;//触摸一次之后按键失效处理
                    return true;
                }
                else if (distance > GoodLeft)
                {
                    //Debug.Log("good");

                    scoreLevel = 0.5f;//good得分
                    Score.Score.totalScore += scoreLevel * sForBar;
                    Score.Score.goodNum += 1;
                    GetComponent<Breakable>().PieceUp();
                    GetComponent<ParticleSystem>().Play();//粒子特效播放
                    QuadPool.Die(gameObject);
                    this.IsValid = false;//触摸一次之后按键失效处理
                    return true;
                }
                //Score.Score.missNum += 1;
                return false;//miss
            }
        }

        return false;//不应该从这里返回  前面涵盖了所有情况

    }
    //*/

    //todo:出边界检测
    public override bool CheckOut()
    {
        if (m_nowPos.z < -5)
        {
            IsValid = false;
            return true;
        }
        else
            return false;
    }

    //todo:统计分数加到计分板上
    /*  函数功能合并到CheckHit（）   此函数弃用
    public void MarkRecording(Score.Score scoreBoard, int ScoreToAdd)
    {

        scoreBoard.totalScore += ScoreToAdd;
        if (ScoreToAdd == 20)//perfect
        {
            scoreBoard.perfectNum++;
        }
        else if (ScoreToAdd == 10)//good
        {
            scoreBoard.goodNum++;
        }
        else//others are miss
        {
            scoreBoard.missNum++;
        }

    }*/


}
