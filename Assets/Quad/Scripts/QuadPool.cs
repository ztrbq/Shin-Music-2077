using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuadPool
{
    public static Stack<GameObject> pool = new Stack<GameObject>();

    public static GameObject Born(Vector3 position, Quaternion quaternion)
    {
        if (pool.Count == 0)
        {
            GameObject q = Resources.Load<GameObject>("Prefabs/Quad");
            GameObject res = MonoBehaviour.Instantiate(q, position, quaternion);
            return res;
        }
        else
        {
            GameObject res = pool.Pop();
            res.SetActive(true);
            return res;
        }
    }

    public static void Die(GameObject gameObject)
    {
        gameObject.SetActive(false);
        QuadBehave behave = gameObject.GetComponent<QuadBehave>();
        if (behave != null)
        {
            MonoBehaviour.Destroy(behave);
        }
        pool.Push(gameObject);
    }
}


