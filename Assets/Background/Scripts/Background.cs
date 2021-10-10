using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Background 
{
    /// <summary>
    /// 请保证一个场景中只存在一个Background预制体。
    /// </summary>
    public class Background : MonoBehaviour
    {
        #region Inspector
        [SerializeField]
        Material[] Slides = new Material[12];
        [SerializeField]
        Material[] Planes = new Material[4];
        [SerializeField]
        Material[] Barriers = new Material[2];
        [SerializeField]
        Transform[] SlideTransforms = new Transform[16];
        [SerializeField]
        Transform[] PlaneTransforms = new Transform[4];
        [SerializeField]
        Light Light = null;
        /// <summary>
        /// 这个属性用于设置四面墙的显示与否,按逆时针顺序,从下方开始为0,true为显示,false为不显示
        /// </summary>
        bool[] PlaneActive = { true, true, true, true };
        [SerializeField]
        bool planesActive = true;
        public bool PlanesActive
        {
            get { return PlaneActive[0]; }
            set { for (int i = 0; i < 4; i++) PlaneActive[i] = value; }
        }
        public static bool PlanesActivity
        {
            get { return Self.PlanesActive; }
            set
            {
                Self.PlanesActive = value;
            }
        }
        float[] ActiveProgress = { 1, 1, 1, 1 };
        Color[] SlideColors = new Color[12];
        #endregion

        #region Self
        static Background Self;
        private void Start()
        {
            for(int i = 0; i < 12; i++)
            {
                SlideColors[i] = Slides[i].GetColor("_Color");
            }
            Self = this;
            StartCoroutine(CheckActivity());
        }
        private void OnValidate()
        {
            PlanesActive = planesActive;
        }
        #endregion

        #region Slides
        /// <summary>
        /// 修改分割线的颜色
        /// </summary>
        /// <param name="index">从左下角开始为0,逆时针旋转,范围是0~11</param>
        /// <param name="color">目标颜色</param>
        public static void SetSlideColor(int index, Color color)
        {
            Self.Slides[index].SetColor("_Color", color);
        }

        /// <summary>
        /// 一次性修改所有分割线的颜色
        /// </summary>
        /// <param name="colors">目标颜色</param>
        public static void SetSlidesColor(Color color)
        {
            for (int i = 0; i < 12; i++)
            {
                SetSlideColor(i, color);
            }
        }

        /// <summary>
        /// 通过一个数组集体修改分割线的颜色,下标从左下角开始为0,逆时针旋转,范围是0~11
        /// </summary>
        /// <param name="colors">目标颜色组,请输入大小为12的数组</param>
        public static void SetSlidesColor(Color[] colors)
        {
            if (colors.Length != 12) throw new System.IndexOutOfRangeException();
            for(int i = 0; i < 12; i++)
            {
                SetSlideColor(i, colors[i]);
            }
        }
        public static void SetSlidesSpec(float intensity)
        {
            for (int i = 0; i < 12; i++)
            {
                float h, s, v;
                Color.RGBToHSV(Self.Slides[i].GetColor("_SpecColor"), out h, out s, out v);
                Self.Slides[i].SetColor("_SpecColor", Color.HSVToRGB(h, s, intensity));
            }
        }
        #endregion

        #region Planes
        /// <summary>
        /// 修改四边挡板的颜色
        /// </summary>
        /// <param name="index">从下方开始为0,逆时针旋转,范围是0~3,左侧是3</param>
        /// <param name="color">目标颜色</param>
        public static void SetPlaneColor(int index, Color color)
        {
            Self.Planes[index].SetColor("_Color", color);
        }

        /// <summary>
        /// 一次性修改所有挡板的颜色
        /// </summary>
        /// <param name="colors">目标颜色</param>
        public static void SetPlanesColor(Color color)
        {
            for (int i = 0; i < 4; i++)
            {
                SetPlaneColor(i, color);
            }
        }

        /// <summary>
        /// 通过一个数组集体修改挡板的颜色,下标从下方开始为0,逆时针旋转,范围是0~3,左侧是3
        /// </summary>
        /// <param name="colors">目标颜色组,请输入大小为4的数组</param>
        public static void SetPlanesColor(Color[] colors)
        {
            if (colors.Length != 4) throw new System.IndexOutOfRangeException();
            for (int i = 0; i < 4; i++)
            {
                SetPlaneColor(i, colors[i]);
            }
        }  
        #endregion

        #region Activity
        IEnumerator CheckActivity()
        {
            for(; ; )
            {
                for(int i = 0; i < 4; i++)
                {
                    if(PlaneActive[i])
                    {
                        ActiveProgress[i] += 0.02f;
                        if (ActiveProgress[i] > 1) ActiveProgress[i] = 1;
                    }
                    else
                    {
                        ActiveProgress[i] -= Time.deltaTime * 0.5f;
                        if (ActiveProgress[i] < 0) ActiveProgress[i] = 0;
                    }
                    Planes[i].SetFloat("_Metallic", (1 - ActiveProgress[i]) * 0.8f);
                    SetBarrierAlpha(i, 1 - ActiveProgress[i]);
                }
                yield return 0;
            }
        }
        static void SetPlaneActive(int index, bool activity)
        {
            switch (index)
            {
                case 0:
                    Self.PlaneTransforms[0].gameObject.SetActive(activity);
                    Self.SlideTransforms[2].gameObject.SetActive(activity);
                    Self.SlideTransforms[3].gameObject.SetActive(activity);
                    Self.SlideTransforms[4].gameObject.SetActive(activity);
                    break;
                case 1:
                    Self.PlaneTransforms[1].gameObject.SetActive(activity);
                    Self.SlideTransforms[7].gameObject.SetActive(activity);
                    break;
                case 2:
                    Self.PlaneTransforms[2].gameObject.SetActive(activity);
                    Self.SlideTransforms[10].gameObject.SetActive(activity);
                    Self.SlideTransforms[11].gameObject.SetActive(activity);
                    Self.SlideTransforms[12].gameObject.SetActive(activity);
                    break;
                case 3:
                    Self.PlaneTransforms[3].gameObject.SetActive(activity);
                    Self.SlideTransforms[15].gameObject.SetActive(activity);
                    break;
            }
        }
        static void SetSlidesAlpha(int index, float t)
        {
            switch (index)
            {
                case 0:
                    Self.Slides[1].SetColor("_Color", Color.Lerp(Self.SlideColors[1], Color.clear, t));
                    Self.Slides[2].SetColor("_Color", Color.Lerp(Self.SlideColors[2], Color.clear, t));
                    Self.Slides[3].SetColor("_Color", Color.Lerp(Self.SlideColors[3], Color.clear, t));
                    break;
                case 1:
                    Self.Slides[5].SetColor("_Color", Color.Lerp(Self.SlideColors[5], Color.clear, t));
                    break;
                case 2:
                    Self.Slides[7].SetColor("_Color", Color.Lerp(Self.SlideColors[7], Color.clear, t));
                    Self.Slides[8].SetColor("_Color", Color.Lerp(Self.SlideColors[8], Color.clear, t));
                    Self.Slides[9].SetColor("_Color", Color.Lerp(Self.SlideColors[9], Color.clear, t));
                    break;
                case 3:
                    Self.Slides[11].SetColor("_Color", Color.Lerp(Self.SlideColors[11], Color.clear, t));
                    break;
            }
        }
        static void SetBarrierAlpha(int index ,float t)
        {
            Color c = Self.Barriers[index * 2].GetColor("_Highcolor");
            Self.Barriers[index * 2].SetColor("_Highcolor", new Color(c.r, c.g, c.b, t));
            c = Self.Barriers[index * 2].GetColor("_Edgecolor");
            Self.Barriers[index * 2].SetColor("_Edgecolor", new Color(c.r, c.g, c.b, t));
            c = Self.Barriers[index * 2 + 1].GetColor("_Highcolor");
            Self.Barriers[index * 2 + 1].SetColor("_Highcolor", new Color(c.r, c.g, c.b, t));
            c = Self.Barriers[index * 2 + 1].GetColor("_Edgecolor");
            Self.Barriers[index * 2 + 1].SetColor("_Edgecolor", new Color(c.r, c.g, c.b, t));
        }

        private void OnDisable()
        {
            for (int i = 0; i < 4; i++)
            {
                SetBarrierAlpha(i, 1); 
                SetSlidesAlpha(i, 0);
                Planes[i].SetFloat("_Metallic", 0);
            }
        }
        #endregion

        #region Transform
        /// <summary>
        /// 将一个物体的放置到一个跑道上,按逆时针顺序,左下角为跑道0,编号为0~11，物体z轴坐标减少为靠近摄像机的方向
        /// </summary>
        /// <param name="transform">要被移动的物体</param>
        /// <param name="index">跑道的序号</param>
        public static void SetPositionAtTrack(Transform transform, int index, float halfLength)
        {            
            switch(index)
            {
                case 0:
                    transform.position = new Vector3(-3.725f, -2.5f, 50.0f+ halfLength-0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 1:
                    transform.position = new Vector3(-1.125f, -2.5f, 50.0f+ halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 2:
                    transform.position = new Vector3(1.125f, -2.5f, 50.0f + halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 3:
                    transform.position = new Vector3(3.725f, -2.5f, 50.0f + halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 4:
                    transform.position = new Vector3(5, -1.25f, 50.0f + halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 5:
                    transform.position = new Vector3(5, 1.25f, 50.0f + halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 6:
                    transform.position = new Vector3(3.725f, 2.5f, 50.0f + halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 7:
                    transform.position = new Vector3(1.125f, 2.5f, 50.0f + halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 8:
                    transform.position = new Vector3(-1.125f, 2.5f, 50.0f + halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 9:
                    transform.position = new Vector3(-3.725f, 2.5f, 50.0f + halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 10:
                    transform.position = new Vector3(-5, 1.25f, 50.0f + halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case 11:
                    transform.position = new Vector3(-5, -1.25f, 50.0f + halfLength - 0.25f);
                    transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Light
        /// <summary>
        /// 设置光光源信息
        /// </summary>
        /// <param name="color">光照颜色</param>
        /// <param name="intensity">光照强度</param>
        public static void SetLight(Color color, float intensity = 2.5f)
        {
            Self.Light.color = color;
            Self.Light.intensity = intensity;
        }
        public static void SetLight(float intensity)
        {
            Self.Light.intensity = intensity;
        }
        #endregion
    }
}

public static class Extension
{
    public static void SetH(this Color c, float hue)
    {
        float h, s, v;
        Color.RGBToHSV(c, out h, out s, out v);
        c = Color.HSVToRGB(hue, s, v);
    }

    public static void SetV(this Color c, float value)
    {
        float h, s, v;
        Color.RGBToHSV(c, out h, out s, out v);
        c = Color.HSVToRGB(h, s, value);
    }
}