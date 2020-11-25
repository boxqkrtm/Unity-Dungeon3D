using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaffRoll : MonoBehaviour
{
    float time = 0;
    void Update()
    {
        time += Time.deltaTime;
        var r = GetComponent<RectTransform>();
        r.anchoredPosition += Vector2.up * Time.deltaTime*100;
        if(115f<=time)
        {
            SceneManager.LoadScene("LoadingScene");
            
        }
    }
}
