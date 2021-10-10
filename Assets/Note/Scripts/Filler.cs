using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filler : MonoBehaviour
{
    Transform parent;
    [SerializeField]
    float boundary => Screen.width == 2160 ? 0.1f : 0.7f;


private void OnEnable()
    {
        parent = transform.parent;
    } 


    void Update()
    {
        float a = Mathf.Clamp01((parent.position.z + parent.lossyScale.z * 0.5f - boundary) / parent.lossyScale.z) * 0.875f;
        transform.localScale = new Vector3(a, 1, 1);
        if(a <= 0.0875f)
        {
            parent.GetComponent<Breakable>().PieceUp(new Vector3(0, 0, 0.3f));
            QuadPool.Die(parent.gameObject);
            QuadMaterial.OnLeave(parent.gameObject);
        }
    }
}
