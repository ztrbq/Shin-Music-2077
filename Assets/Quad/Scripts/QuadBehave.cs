using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Score;
using Note;

public class QuadBehave : MonoBehaviour
{
    public delegate void hitManage();//特效处理
    public event hitManage hitQuad;//击中事件
    protected Vector3 m_nowPos;
    public bool IsValid { get; set; } = true;
    //load prefab
    protected GameObject m_quad;

    protected Note.Note m_note = new Note.Note(NoteType.NoteStrip,10,2);
    
    
    [SerializeField]
    static protected float erreurGood=0.9f, erreurPerfect=0.25f;

  
    static public float m_vel = 10f;

    static public float GoodLeft { get { return Settings.Settings.SurfacePos - 4 * erreurGood; } }
    static public float GoodRight { get { return Settings.Settings.SurfacePos + 2 * erreurGood; } }
    static public float PerfectLeft { get { return Settings.Settings.SurfacePos - 6 * erreurPerfect; } }
    static public float PerfectRight { get { return Settings.Settings.SurfacePos + erreurPerfect; } }

    //use this to init
    virtual public void Initialize(Note.Note note) { }

    // Start is called before the first frame update
    protected void Start()
    {
        m_quad = this.gameObject;
        Background.Background.SetPositionAtTrack(transform, m_note.trackNum,this.transform.localScale.z/2);
        m_nowPos = transform.position;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        m_nowPos.Set(m_nowPos.x, m_nowPos.y, m_nowPos.z - m_vel * Time.deltaTime);
        this.transform.position = m_nowPos;
        if (CheckHit())
        {
            //Debug.Log("真的点中了");
            //hitQuad();
        }
        if (CheckOut())
        {
            //Debug.Log("checkout");
            QuadPool.Die(this.gameObject);
        }
        
    }

    //todo:检测触摸判断
    virtual public bool CheckHit()
    {
        return false;
    }


    virtual public bool CheckOut()
    {
        return false;
    }



    static public void SetErreur(float goo, float per)
    {
        erreurGood = goo;
        erreurPerfect = per;
    }
    static public void SetVel(float vel)
    {
        m_vel = vel;
    }


    public Ray[] getAllRaysThroughTouches()  //这个函数获得所有从相机出发，经过手机屏幕上触摸点的Unity世界中的射线
    {
        //Debug.Log("触摸个数"+Input.touches.Length);
        Vector3[] touchPositions = new Vector3[Input.touches.Length];//把像素坐标存成Vector3，z为0，因为z在后边会自动忽略       
        for (int i = 0; i < Input.touches.Length; i++)
        {
            touchPositions[i].x = Input.touches[i].position.x;
            touchPositions[i].y = Input.touches[i].position.y;
            touchPositions[i].z = 0;
        }
        Ray[] allRays = new Ray[Input.touches.Length];
        for (int i = 0; i < Input.touches.Length; i++)
        {
            allRays[i] = Camera.main.ScreenPointToRay(touchPositions[i]);
        }
        return allRays;    
    }

    static bool[] trackStates = new bool[12];
    public static bool frameChecked = false;

    //判断是否按下了指定通道的按键,tracknum是要判断的哪个通道上的按键
    public bool haveTouchOfTheTrack(int tracknum)
    {
        if (frameChecked) return trackStates[tracknum];
        Ray[] allRays = this.getAllRaysThroughTouches();
        RaycastHit[] raycastHits = new RaycastHit[allRays.Length];
        Vector3[] touchPositions = new Vector3[allRays.Length];//这个数组存放射线与墙壁碰撞点
        for(int i = 0; i < Input.touches.Length; i++)
        {
            if (Physics.Raycast(allRays[i], out raycastHits[i]))
            {
                touchPositions[i] = raycastHits[i].point;
                //Debug.Log("碰撞点的位置为：("+touchPositions[i].x+","+ touchPositions[i].y+","+ touchPositions[i].z+")");
            }
            else
                touchPositions[i] = new Vector3(100, 100, 100);//没什么影响，肯定判断出来是没按下按键
        }
        for (int i = 0; i < 12; i++) trackStates[i] = false;
        foreach (Vector3 touch in touchPositions)
        {
            if(touch.z < Settings.Settings.SurfacePos && touch.x > -5 && touch.x < -2.5 && touch.y == -2.6f) trackStates[0] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.x > -2.5 && touch.x < 0 && touch.y == -2.6f) trackStates[1] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.x > 0 && touch.x < 2.5 && touch.y == -2.6f) trackStates[2] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.x > 2.5 && touch.x < 5 && touch.y == -2.6f) trackStates[3] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.y > -2.5 && touch.y < 0 && Math.Abs(touch.x - 5.099999f) < 0.000001) trackStates[4] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.y > 0 && touch.y < 2.5 && touch.x == 5.1f) trackStates[5] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.x > 2.5 && touch.x < 5 && touch.y == 2.6f) trackStates[6] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.x > 0 && touch.x < 2.5 && touch.y == 2.6f) trackStates[7] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.x > -2.5 && touch.x < 0 && touch.y == 2.6f) trackStates[8] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.x > -5 && touch.x < -2.5 && touch.y == 2.6f) trackStates[9] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.y > 0 && touch.y < 2.5 && touch.x == -5.1f) trackStates[10] = true;
            if (touch.z < Settings.Settings.SurfacePos && touch.y > -2.5 && touch.y < 0 && (Math.Abs(touch.x + 5.099999f) < 0.000001)) trackStates[11] = true;
        }
        return trackStates[tracknum];
    }
}

