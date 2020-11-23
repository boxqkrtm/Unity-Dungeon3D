using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 db 전체를 가지는 객체 싱글턴
//ItemManager.Instance.CodeToItem(itemCode) 으로 아이템 객체 반환

public class ItemManager
{
    static private ItemManager instance = null;
    static public ItemManager Instance
    {
        get
        {
            //Debug.Log("call instance itemmanager");
            if (instance == null)
                instance = new ItemManager();
            return instance;
        }
    }


    private List<Item> items;

    public Item CodeToItem(int itemCode)
    {
        return new Item(items[itemCode]);
    }

    private ItemManager()
    {
        items = new List<Item>();
        var itemCSV = CSVReader.Read("items");
        foreach (var elem in itemCSV)
        {
            //Debug.Log(int.Parse(elem["itemCode"].ToString()));
            //Debug.Log((Resources.Load(elem["itemIcon"].ToString()) as Texture2D).);
            //Debug.Log(elem["itemName"].ToString());
            //Debug.Log(elem["itemDescription"].ToString());
            //Debug.Log(StringToItemType(elem["itemType"].ToString()));
            //Debug.Log((elem["itemPower"].ToString()));
            //Debug.Log(int.Parse(elem["resistFire"].ToString()));
            //Debug.Log(int.Parse(elem["resistIce"].ToString()));
            //Debug.Log(int.Parse(elem["resistLight"].ToString()));
            //Debug.Log(int.Parse(elem["resistPoison"].ToString()));
            //Debug.Log(1);
            //Debug.Log(int.Parse(elem["maxItemAmount"].ToString()));
            var item = new Item(
                int.Parse(elem["itemCode"].ToString()),
                Sprite.Create(Resources.Load(elem["itemIcon"].ToString()) as Texture2D, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f), 256f),
                elem["itemName"].ToString(),
                elem["itemDescription"].ToString(),
                StringToItemType(elem["itemType"].ToString()),
                float.Parse(elem["itemPower"].ToString()),
                int.Parse(elem["resistFire"].ToString()),
                int.Parse(elem["resistIce"].ToString()),
                int.Parse(elem["resistLight"].ToString()),
                int.Parse(elem["resistPoison"].ToString()),
                1,
                int.Parse(elem["maxItemAmount"].ToString()),
                int.Parse(elem["itemPrice"].ToString())
                );
            items.Add(item);
        }
    }

    ItemType StringToItemType(string str)
    {
        return (ItemType)Enum.Parse(typeof(ItemType), str);
    }
}
