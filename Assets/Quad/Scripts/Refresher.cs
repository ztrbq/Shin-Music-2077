using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refresher : MonoBehaviour
{
    [SerializeField]
    Material[] m;

    private void Start()
    {
        foreach(Material mt in m)
        {
            mt.SetFloat("_Boundary", Settings.Settings.SurfacePos); 
        }
    }

    void LateUpdate()
    {
        QuadBehave.frameChecked = false;
    }
}
