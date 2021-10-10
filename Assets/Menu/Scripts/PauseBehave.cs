using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehave : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;


    static public bool isPause = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

}
