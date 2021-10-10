using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Background;

public class BackgroundColor : MonoBehaviour
{
    [SerializeField]
    Vector3 PlaneHSV = new Vector3(0.6f, 0.2f, 0.6f);

    // Update is called once per frame
    void Update()
    {
        Background.Background.SetPlanesColor(Color.HSVToRGB(PlaneHSV.x + Mathf.Cos(Time.time) * 0.1f, PlaneHSV.y, PlaneHSV.z));
        Background.Background.SetSlidesSpec(0.6f + Mathf.Cos(Time.time * 2) * 0.4f);
    }

    private void OnDisable()
    {
        Background.Background.SetPlanesColor(Color.HSVToRGB(PlaneHSV.x, PlaneHSV.y, PlaneHSV.z));
        Background.Background.SetSlidesSpec(1);
        
    }
}
