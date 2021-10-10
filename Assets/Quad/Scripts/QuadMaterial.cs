using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadMaterial
{
    public static Material normalMat = Resources.Load<Material>("Materials\\LongNodeNormal");
    public static Material fillMat = Resources.Load<Material>("Materials\\LongNodeFill");

    static Stack<GameObject> pool = new Stack<GameObject>();
    static GameObject CreateFiller(Transform parent)
    {
        GameObject go;
        if (pool.Count > 0)
        {
            go = pool.Pop();
            go.transform.SetParent(parent);
            go.SetActive(true);
        }
        else
        {
            go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs\\Filler"), parent);
        }
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.Euler(Vector3.zero);
        return go;
    }
    static void DestroyFiller(Transform filler)
    {
        pool.Push(filler.gameObject);
        filler.gameObject.SetActive(false);
    }

    public static void OnTouch(GameObject quad)
    {
        if (quad.transform.childCount > 0) return;
        quad.GetComponent<MeshRenderer>().sharedMaterial = fillMat;
        CreateFiller(quad.transform);
    }

    public static void OnLeave(GameObject quad)
    {
        if(quad.transform.childCount > 0) DestroyFiller(quad.transform.GetChild(0));
        quad.GetComponent<MeshRenderer>().sharedMaterial = normalMat;
    }
}
