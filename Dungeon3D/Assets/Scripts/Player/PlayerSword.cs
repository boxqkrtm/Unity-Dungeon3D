using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Monster"))
        {
            GameManager.Instance.PlayerSwordHit(col.gameObject, col.transform.position);
        }
    }
}
