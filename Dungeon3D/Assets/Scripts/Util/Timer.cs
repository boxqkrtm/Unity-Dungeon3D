using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Timer
{
    //변수를 넣으면 deltaTime만큼 감소시킴
    //단 0보다 작은 경우 0으로 고정
    public static void Update(ref float x)
    {
        if (x > 0)
        {
            x -= Time.deltaTime;
        }
        else
        {
            x = 0;
        }
    }
}
