using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public int manualItemCode = 0;
    public bool isManualSpawn = false;
    Item itemData;
    public void SetItem(Item item)
    {
        itemData = item;
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = itemData.ItemIcon;
    }
    private void Start()
    {
        if (isManualSpawn == true)
            SetItem(ItemManager.Instance.CodeToItem(manualItemCode));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            itemData.ItemAmount = InventoryManager.Instance.GetItem(itemData);
            //Debug.Log("hit remain " + itemData.ItemAmount);
            if (itemData.ItemAmount == 0) Destroy(gameObject);
        }
    }
}
