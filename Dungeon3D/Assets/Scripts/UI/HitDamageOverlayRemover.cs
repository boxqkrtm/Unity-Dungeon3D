using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitDamageOverlayRemover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetText(string txt)
    {
        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = txt;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;
        var tcolor = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(tcolor.r, tcolor.g, tcolor.b, tcolor.a - 1f * Time.deltaTime);
        if(tcolor.a <= 0.01f)
        {
            Destroy(gameObject);
        }
        
    }
}
