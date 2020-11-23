using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Loading : MonoBehaviour
{
    public Image LoadBar;
    string targetSceneName;
    AsyncOperation aop;
    float smooth;
    void Start()
    {
        var gameSavePath = Application.persistentDataPath + "/save.dat";
        string targetSceneName = "MainTown";
        try
        {
            var data = GameManager.LoadGameData(gameSavePath);
            targetSceneName = data.ud.SceneName;
        }
        catch
        {
            targetSceneName = "MainTown";
        }
        aop = SceneManager.LoadSceneAsync(targetSceneName);
        aop.allowSceneActivation = false;
        smooth = 0f;
        GameObject.Find("Fade").GetComponent<Fade>().FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        smooth = Mathf.MoveTowards(smooth, Mathf.Clamp01(aop.progress / 0.9f), 1f * Time.deltaTime);
        LoadBar.fillAmount = smooth;
        if (smooth >= 0.99f)
        {
            StartCoroutine(SceneMove());
        }
    }

    IEnumerator SceneMove()
    {
        GameObject.Find("Fade").GetComponent<Fade>().FadeOut();
        yield return new WaitForSeconds(0.5f);
        aop.allowSceneActivation = true;
    }
}