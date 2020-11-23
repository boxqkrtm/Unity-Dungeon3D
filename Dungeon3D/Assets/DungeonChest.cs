using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonChest : MonoBehaviour
{
    public List<int> manualItemCodes = new List<int>();
    public bool isManualSpawn = false;
    public List<Item> Items{get => items; set=>items = value;}
    List<Item> items;

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            StartCoroutine(OpenChest());
        }
    }

    IEnumerator OpenChest()
    {
        yield return null;
        GetComponent<Animator>().SetBool("OpenChest", true);
        yield return new WaitForSeconds(1f);
        if (isManualSpawn == false)
        {
            foreach (Item item in items)
                EffectManager.Instance.CreateDroppedItem(transform.position, item);
        }
        else
        {
            foreach (int code in manualItemCodes)
            {
                EffectManager.Instance.CreateDroppedItem(transform.position,
                ItemManager.Instance.CodeToItem(code));
            }
        }
        yield return new WaitForSeconds(0.5f);
        EffectManager.Instance.CreateMobDespawnEffect(transform.position);
        Destroy(gameObject);
    }
}
