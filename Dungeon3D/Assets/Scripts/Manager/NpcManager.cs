using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NpcManager : MonoBehaviour
{
    public GameObject interactWindow;//chat window
    //prefab
    public GameObject optionPrefab;
    public GameObject shopSlot;
    //shop
    public GameObject shopGUI;
    public ShopBuyAlert shopBuyAlert;

    public int OptionNumber { get; private set; }
    //-1 미선택 대기중
    //-2 창없음

    public static NpcManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        OptionNumber = -2;
    }

    //대화 또는 선택지를 이용하는 창
    public void CreateInteractWindow(string text, List<NpcTalkOption> options)
    {
        OptionNumber = -1;
        interactWindow.SetActive(true);
        interactWindow.transform.GetChild(0).GetComponent<Text>().text = text;

        for (var i = 0; i < interactWindow.transform.GetChild(1).childCount; i++)
            Destroy(interactWindow.transform.GetChild(1).GetChild(i).gameObject);

        for (var i = 0; i < options.Count; i++)
        {
            var o = Instantiate(optionPrefab);
            o.transform.SetParent(interactWindow.transform.GetChild(1).transform);
            o.transform.localScale = Vector3.one;
            o.transform.GetChild(0).GetComponent<Text>().text = options[i].optionName;
            var oNum = i;
            o.GetComponent<Button>().onClick.AddListener(() => OptionNumber = oNum);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)interactWindow.transform.GetChild(1));
    }

    public void CloseInteractWindow()
    {
        interactWindow.SetActive(false);
        OptionNumber = -2;
    }

    public void OpenShopWindow(List<Item> showcaseList)
    {
        shopGUI.SetActive(true);
        var shopGrid = shopGUI.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);

        for (var i = 0; i < shopGrid.childCount; i++)
            Destroy(shopGrid.GetChild(i).gameObject);

        foreach (var i in showcaseList)
        {
            var slot = Instantiate(shopSlot);
            slot.transform.SetParent(shopGrid);
            slot.transform.localScale = Vector3.one;
            var Icon = slot.transform.GetChild(0).Find("Icon");
            var Price = slot.transform.GetChild(0).GetChild(1).Find("Price");
            var Info = slot.transform.GetChild(0).GetChild(1).Find("Info");
            var Name = slot.transform.GetChild(0).GetChild(1).Find("Name");
            Icon.GetComponent<Image>().sprite = i.ItemIcon;
            Price.GetComponent<Text>().text = i.ItemPrice.ToString() + "G";
            Info.GetComponent<Text>().text = i.ItemDescription.ToString();
            Name.GetComponent<Text>().text = i.ItemName.ToString();
            var slotItem = i;
            slot.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    shopBuyAlert.OpenAlert(slotItem);
                }
            );
        }
    }

    public void CloseShopWindow()
    {
        shopGUI.SetActive(false);
    }
}