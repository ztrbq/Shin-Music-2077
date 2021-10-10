using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    GameObject number1;
    GameObject number2;
    GameObject number3;
    GameObject start;
    AudioSource AS;
    public static bool flag = false;//表明音乐是否已经开始播放
    private void Start()
    {
        flag = false;
        AS = gameObject.GetComponent<AudioSource>();
        AS.clip = Resources.Load<AudioClip>(Settings.Settings.ChosenSong);

        GameObject q = Resources.Load<GameObject>("Prefabs/number1Text");
        number1 = MonoBehaviour.Instantiate(q, new Vector3(0, 0, 50.0f), new Quaternion());

        q = Resources.Load<GameObject>("Prefabs/number2Text");
        number2 = MonoBehaviour.Instantiate(q, new Vector3(0, 0, 50.0f), new Quaternion());

        q = Resources.Load<GameObject>("Prefabs/number3Text");
        number3 = MonoBehaviour.Instantiate(q, new Vector3(0, 0, 50.0f), new Quaternion());

        q = Resources.Load<GameObject>("Prefabs/startText");
        start = MonoBehaviour.Instantiate(q, new Vector3(0, 0, 50.0f), new Quaternion());
        AS.Stop();
        StartCoroutine(Count());
    }
    public IEnumerator Count()
    {
        number1.transform.position = new Vector3(0, 0, 50);
        number2.transform.position = new Vector3(0, 0, 50);
        number3.transform.position = new Vector3(0, 0, 50);
        start.transform.position = new Vector3(0, 0, 50);
        Time.timeScale = 0;

        for (float t = 0; t <= 55.0f; t += 1f)
        {
            number3.transform.position -= new Vector3(0, 0, 1f);
            yield return 0;
        }
        for (float t = 0; t <= 55.0f; t += 1f)
        {
            number2.transform.position -= new Vector3(0, 0, 1f);
            yield return 0;
        }
        for (float t = 0; t <= 55.0f; t += 1f)
        {
            number1.transform.position -= new Vector3(0, 0, 1f); ;
            yield return 0;
        }
        for (float t = 0; t <= 55.0f; t += 1f)
        {
            start.transform.position -= new Vector3(0, 0, 1f);
            yield return 0;
        }
        Time.timeScale = 1;
        Background.Background.PlanesActivity = false;
        flag = true;
        AS.Play();

    }
}
