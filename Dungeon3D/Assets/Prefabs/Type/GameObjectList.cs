using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectList
{
    List<GameObject> list;
    public GameObjectList()
    {
        list = new List<GameObject>();
    }
    public void Add(GameObject obj)
    {
        list.Add(obj);
    }
    public void RemoveAt(int index)
    {
        GameManager.Instance.DestroyGameObject(list[index]);
        list.RemoveAt(index);
    }
    public GameObject this[int index]
    {
        get => list[index];
        set => list[index] = value;
    }
    public int Count
    {
        get => list.Count;
    }
}
