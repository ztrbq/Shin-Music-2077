using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    public static void Log(string msg)
    {
        GameObject.Find("Debugger").GetComponent<Text>().text += "\n" + msg;
    }
}
