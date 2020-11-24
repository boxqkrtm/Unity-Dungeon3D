using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffRoll : MonoBehaviour
{
    void Update()
    {
        var r = GetComponent<RectTransform>();
        r.anchoredPosition += Vector2.up * Time.deltaTime*100;
    }
}
