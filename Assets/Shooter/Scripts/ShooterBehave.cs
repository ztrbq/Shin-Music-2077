using Note;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShooterBehave : MonoBehaviour
{
    float time;
    private int i;    //对List列表进行计数
    private bool[] Track;        //存储12条通道是否使用的数组
    private List<Note.Note> Music;   //深复制存储乐谱的链表
    [SerializeField]
    private AudioSource As;
    float startTime = 0.0f;
    private void Shoot()    //根据乐谱链表，发射一个Quad
    {
        switch (Music[i].noteType)  //按类型绑定脚本
        {
            case Note.NoteType.NoteBar:       
                GameObject squad = QuadPool.Born(new Vector3(), new Quaternion());   //生成quad
                squad.transform.localScale = new Vector3(2, 0.02f, 0.5f);
                squad.GetComponent<BoxCollider>().enabled = true;
                ShortQuadBehave sqb = squad.AddComponent<ShortQuadBehave>();
                sqb.Initialize((Note.Note_NoteBar)Music[i]);
                break;
            case Note.NoteType.NoteStrip:
                GameObject lquad = QuadPool.Born(new Vector3(), new Quaternion());   //生成quad
                lquad.transform.localScale = new Vector3(2, 0.02f,((Note.Note_NoteStrip)Music[i]).lastTime * QuadBehave.m_vel);
                lquad.GetComponent<BoxCollider>().enabled = false;
                LongQuadBehave lqb = lquad.AddComponent<LongQuadBehave>();
                lqb.Initialize((Note.Note_NoteStrip)Music[i]);
                break;
            default: break;
        }
    }
           
    void Start()    //初始化游戏的时间和乐谱音符计数
    {       
        //As = GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
        MusicScore.MusicScoreManager.ImportFromJSON(Settings.Settings.ChosenSong);
        this.ImportMusic(MusicScore.MusicScoreManager.musicScore.musicScore);
        switch (Settings.Settings.difficulty)
        {
            case Settings.Difficulty.easy:
                Settings.Settings.velocity = MusicScore.MusicScoreManager.musicScore.easyVel;
                QuadBehave.m_vel = Settings.Settings.velocity;
                break;
            case Settings.Difficulty.difficult:
                Settings.Settings.velocity = MusicScore.MusicScoreManager.musicScore.difficultVel;
                QuadBehave.m_vel = Settings.Settings.velocity;
                break;
            default:
                Settings.Settings.velocity = MusicScore.MusicScoreManager.musicScore.easyVel;
                QuadBehave.m_vel = Settings.Settings.velocity;
                break;
        }
        i = 0;
        time = 50 / QuadBehave.m_vel;
        startTime = Time.time;
    }   
    void FixedUpdate()    //根据时间判断发射与否
    {       
        if (i < Music.Count)
        {       
            Note.Note currentNote = Music[i];
            if (Time.time >= currentNote.arrivalTime - time + startTime)
            {
                if (i == 0) Background.Background.PlanesActivity = false;
                Shoot();
                i++;
            }
        }
        if (!As.isPlaying&&CountDown.flag==true)
        {
            MusicEnd();
        }
    }

    void MusicEnd()
    {
        SceneManager.LoadScene("Clearing");
    }

    private void ImportMusic(List<Note.Note> OriginMusic) 
    {
        using (MemoryStream ms = new MemoryStream())  //使用序列化+反序列化进行深复制
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, OriginMusic);
            ms.Position = 0;
            Music = (List<Note.Note>)bf.Deserialize(ms);
        }
    }
}

