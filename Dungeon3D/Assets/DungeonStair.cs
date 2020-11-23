using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStair : MonoBehaviour
{
    float delay = 0f;
    bool isPlayerStay = false;
    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isPlayerStay = true;
            if (delay == 0)
            {
                AlertManager.Instance.AddGameLog("계단에 3초 더 있으면 이동합니다.");
            }
            delay += Time.deltaTime;
            if (delay >= 3f)
            {

                EffectManager.Instance.FadeOut();
                GameManager.Instance.UpStairDungeon();
            }
        }
    }
    void FixedUpdate()
    {
        if (isPlayerStay == false)
        {
            delay = 0;
        }
        isPlayerStay = false;
    }
}
