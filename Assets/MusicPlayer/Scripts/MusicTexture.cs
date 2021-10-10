using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicSampler;

public class MusicTexture : MonoBehaviour
{
    SpriteRenderer[] frames = new SpriteRenderer[16];
    [SerializeField]
    uint count = 12;
    [SerializeField]
    float Radius = 1, width = 1, height = 1, amplitude = 2;

    [SerializeField]
    SpriteRenderer Frame = null;

    [SerializeField]
    MusicSampler.MusicSampler sampler = null;

    [SerializeField]
    Material[] materials = new Material[2];

    [SerializeField]
    Material Disortion = null;

    [SerializeField]
    Vector3 HighColorHSV;
    [SerializeField]
    Vector3 EdgeColorHSV;
    // Start is called before the first frame update
    void Start()
    {
        Score.Score.Reset();
        frames = new SpriteRenderer[count];
        CreateFrames();
    }

    void CreateFrames()
    {
        float single = 6.28f / count;
        for (int i = 0; i < count; i++)
        {
            float rot = single * i;
            Vector3 position = new Vector3(Mathf.Cos(rot), Mathf.Sin(rot), 0) * Radius;
            frames[i] = Instantiate(Frame, transform);
            frames[i].transform.localPosition = position;
            frames[i].transform.localRotation = Quaternion.Euler(0, 0, 360f / count * i);
            frames[i].size = new Vector2(height, width);
        }
    }

    void Update()
    {
        for(int i = 0; i < count; i++)
        {
            frames[i].size = new Vector2(height * (1 + sampler.normalizedBands[i] * amplitude), width);
        }
        HighColorHSV += Vector3.right * 0.002f;
        if (HighColorHSV.x > 1) HighColorHSV -= Vector3.right;
        EdgeColorHSV += Vector3.right * 0.002f;
        if (EdgeColorHSV.x > 1) EdgeColorHSV -= Vector3.right;
        for (int i = 0; i < materials.Length; i ++ )
        {
            materials[i].SetColor("_Color", Color.HSVToRGB(HighColorHSV.x, HighColorHSV.y, HighColorHSV.z));
            materials[i].SetColor("_Highcolor", Color.HSVToRGB(HighColorHSV.x, HighColorHSV.y, HighColorHSV.z));
            materials[i].SetColor("_Edgecolor", Color.HSVToRGB(EdgeColorHSV.x, EdgeColorHSV.y, EdgeColorHSV.z));
        }
        Disortion.SetFloat("_NoiseStrength", sampler.average * 0.05f);
    }

    private void OnDisable()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_Color", Color.HSVToRGB(0, HighColorHSV.y, HighColorHSV.z));
            materials[i].SetColor("_Highcolor", Color.HSVToRGB(0, HighColorHSV.y, HighColorHSV.z));
            materials[i].SetColor("_Edgecolor", Color.HSVToRGB(0.5f, EdgeColorHSV.y, EdgeColorHSV.z));
        }
        Disortion.SetFloat("_NoiseStrength", 0);
    }
}
