using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    float fade = 0f;
    Image fadeImg;
    void Start()
    {
        fadeImg = transform.GetChild(0).GetComponent<Image>();
        fadeImg.color = Color.black;
        StartCoroutine(FadeR());
    }
    public void FadeIn()
    {
        fade = 0f;
    }

    IEnumerator FadeR()
    {
        while (true)
        {
            yield return null;
            var bef = fadeImg.color;
            var alpha = Mathf.MoveTowards(bef.a, fade, 2f * Time.deltaTime);
            fadeImg.color = new Color(bef.r, bef.g, bef.b, alpha);
        }
    }

    public void FadeOut()
    {
        fade = 1f;
    }
}
