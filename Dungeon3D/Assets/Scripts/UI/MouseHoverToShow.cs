using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHoverToShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Text t;
    Color tc;
    void Start()
    {
        t = transform.GetChild(0).GetComponent<Text>();
        tc = t.color;
        t.color = Color.clear;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        t.color = tc;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        t.color = Color.clear;
    }
}
