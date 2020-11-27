using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region Variable
    //prefab
    public GameObject itemSlot;
    public GameObject quickSlot;
    public Sprite blank;

    //sprite
    public Sprite blankSword;
    public Sprite blankArmor;
    public Sprite blankAccessory;

    //link
    public GameObject inventoryUI;
    public GameObject inventoryGrid;
    public GameObject eArmor;
    public GameObject eAccessory;
    public GameObject eSword;
    public Text eGold;
    public GameObject floatItem;// 드래그 아이템 오버레이
    public GameObject floatInfo;// 아이템 가져다 대면 아이템 정보 표시
    List<GameObject> invSlots = new List<GameObject>();
    Player pscript;
    public Player PlayerScript
    {
        get
        {
            if (pscript == null) pscript = GameManager.Instance.PlayerObj.GetComponent<Player>();
            return pscript;
        }
    }
    public List<Item> PlayerItems { get => PlayerScript.ud.Items; }
    Coroutine OpenInvAnimRoutine = null;
    Coroutine CloseInvAnimRoutine = null;
    float invAnimSpeed = 8f;
    private static InventoryManager instance = null;
    public static InventoryManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion
    #region Init
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        eSword.GetComponent<InventorySlotListener>().canvas = GameManager.Instance.MainUICanvas;
        eArmor.GetComponent<InventorySlotListener>().canvas = GameManager.Instance.MainUICanvas;
        eAccessory.GetComponent<InventorySlotListener>().canvas = GameManager.Instance.MainUICanvas;
        eSword.GetComponent<InventorySlotListener>().myIndex = -2;
        eArmor.GetComponent<InventorySlotListener>().myIndex = -3;
        eAccessory.GetComponent<InventorySlotListener>().myIndex = -4;
        CloseInvAnimRoutine = StartCoroutine(CloseInvAnim());
    }
    #endregion
    #region InvOnOff
    IEnumerator OpenInvAnim()
    {
        UIUpdate();
        yield return null;
        inventoryUI.SetActive(true);
        inventoryUI.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        var startSize = Vector3.zero;
        var endSize = Vector3.one;
        inventoryUI.transform.localScale = startSize;
        while (Vector3.Distance(inventoryUI.transform.localScale, endSize) > 0.5f)
        {
            yield return null;
            inventoryUI.transform.localScale = Vector3.Lerp(inventoryUI.transform.localScale, endSize, Time.deltaTime * 1 / Time.timeScale * invAnimSpeed);
        }
        inventoryUI.transform.localScale = endSize;
        OpenInvAnimRoutine = null;
        SEManager.Instance.Play(SEManager.Instance.buttonSE);
    }
    IEnumerator CloseInvAnim()
    {
        if(inventoryUI.activeSelf == true)
        SEManager.Instance.Play(SEManager.Instance.closeSE);
        UIUpdate();
        yield return null;
        var startSize = Vector3.one;
        var endSize = Vector3.zero;
        inventoryUI.transform.localScale = startSize;
        while (Vector3.Distance(inventoryUI.transform.localScale, endSize) > 0.5f)
        {
            yield return null;
            inventoryUI.transform.localScale = Vector3.Lerp(inventoryUI.transform.localScale, endSize, Time.deltaTime * 1 / Time.timeScale * invAnimSpeed);
        }
        inventoryUI.transform.localScale = endSize;
        inventoryUI.SetActive(false);
        CloseInvAnimRoutine = null;
    }
    public void OpenInventory()
    {
        OpenInvAnimRoutine = OpenInvAnimRoutine ?? StartCoroutine(OpenInvAnim());
    }
    public void CloseInventory()
    {
        CloseInvAnimRoutine = CloseInvAnimRoutine ?? StartCoroutine(CloseInvAnim());
    }
    public void ToggleInventory()
    {
        if (inventoryUI.activeSelf == true) CloseInventory();
        else OpenInventory();
    }
    public bool IsInventoryOpened()
    {
        return inventoryUI.activeSelf;
    }
    #endregion
    #region InvItemBehavior
    public void SwapItem(int a, int b)
    {
        Debug.Log("swap" + a.ToString() + " " + b.ToString());
        if (a <= -2)
        {
            //장비류 아이템 해제
            UseItem(a);
            return;
        }
        if (b <= -2 && b >= -4)
        {
            //장비류 아이템은 장착
            UseItem(a);
            return;
        }
        if (b <= -5 && b >= -8)
        {
            //퀵슬롯 설정
            PlayerScript.ud.quickslot[b * -1 - 5] = a;
            UIUpdate();
            return;
        }
            if (a == b) return;//이게 없으면 같은 자리로 옮기면 아이템 사라짐
        if (PlayerItems[a].ItemCode == PlayerItems[b].ItemCode &&
            PlayerItems[b].ItemMaxAmount != PlayerItems[b].ItemAmount)
        {
            var canInputAmount = PlayerItems[b].ItemMaxAmount - PlayerItems[b].ItemAmount;
            if (canInputAmount > PlayerItems[a].ItemAmount)
            {
                PlayerItems[b].ItemAmount += PlayerItems[a].ItemAmount;
                PlayerItems[a].ItemAmount = 0;
            }
            else
            {
                PlayerItems[a].ItemAmount -= canInputAmount;
                PlayerItems[b].ItemAmount += canInputAmount;
            }
        }
        else
        {
            var temp = PlayerItems[a];
            PlayerItems[a] = PlayerItems[b];
            PlayerItems[b] = temp;
        }
        UIUpdate();
    }
    public void SplitItem(int a, int b)
    {
        //Debug.Log("Split");
        if (PlayerItems[b].ItemCode == 0)
        {
            var halfV = PlayerItems[a].ItemAmount / 2;
            PlayerItems[a].ItemAmount -= halfV;
            PlayerItems[b] = new Item(PlayerItems[a]);
            PlayerItems[b].ItemAmount = halfV;
        }
        UIUpdate();
    }
    public void UseItem(int a)
    {
        if (a>=0 && PlayerScript.ud.Items[a].ItemCode == 0) return;
        HideItemInfo();
        //장비칸에서 더블클릭 한 경우 장착 해제
        if (a <= -2)
        {
            if (a == -2)
            {
                if (GetItem(PlayerScript.ud.Weapon, false) == 0)
                {
                    PlayerScript.ud.Weapon = new Item(0);
                    SEManager.Instance.Play(SEManager.Instance.useSE);
                    UIUpdate();
                    return;
                }
                else
                {
                    //장비 해제 실패 인벤토리 가득 참
                    return;
                }
            }
            if (a == -3)
            {
                if (GetItem(PlayerScript.ud.Armor, false) == 0)
                {
                    PlayerScript.ud.Armor = new Item(0);
                    SEManager.Instance.Play(SEManager.Instance.useSE);
                    UIUpdate();
                    return;
                }
                else
                {
                    //장비 해제 실패 인벤토리 가득 참
                    return;
                }
            }
            if (a == -4)
            {
                if (GetItem(PlayerScript.ud.Accessory, false) == 0)
                {
                    PlayerScript.ud.Accessory = new Item(0);
                    SEManager.Instance.Play(SEManager.Instance.useSE);
                    UIUpdate();
                    return;
                }
                else
                {
                    //장비 해제 실패 인벤토리 가득 참
                    return;
                }
            }
            if(a <= -5)
            {
                UseItem(PlayerScript.ud.quickslot[a * -1 - 5]);
                SEManager.Instance.Play(SEManager.Instance.useSE);
                return;
            }
        }

        //무기인 경우 장착
        if (PlayerItems[a].ItemType == ItemType.Sword)
        {
            var temp = PlayerScript.ud.Weapon;
            PlayerScript.ud.Weapon = PlayerItems[a];
            PlayerItems[a] = temp;
            UIUpdate();
            return;
        }
        if (PlayerItems[a].ItemType == ItemType.Armor)
        {
            var temp = PlayerScript.ud.Armor;
            PlayerScript.ud.Armor = PlayerItems[a];
            PlayerItems[a] = temp;
            UIUpdate();
            return;
        }
        if (PlayerItems[a].ItemType == ItemType.Accessories)
        {
            var temp = PlayerScript.ud.Accessory;
            PlayerScript.ud.Accessory = PlayerItems[a];
            PlayerItems[a] = temp;
            UIUpdate();
            return;
        }

        //아이템을 사용 처리함
        PlayerItems[a].Use(PlayerScript.ud);
        SEManager.Instance.Play(SEManager.Instance.useSE);
        UIUpdate();
    }
    public void DropItem(int a)
    {
        PlayerItems[a].Drop();
        UIUpdate();
    }
    //아이템 획득함수
    //int는 들어가지 못한 아이템의 수량 반환 ex 1 = 1개 아이템창 들어가기 실패
    public bool TestGetItem(Item getItem)
    {
        var copiedInv = new List<Item>(); //깊은 복사 참조
        foreach(var i in PlayerItems)
            copiedInv.Add(new Item(i));
        if (GetItem(getItem, false, copiedInv) == 0)
        {
            return true;
        }
        return false;
    }
    public int GetItem(Item getItem, bool alert = true, List<Item> playerInv = null)
    {
        if (playerInv == null) 
        {
            //Debug.Log("Player");
            playerInv = PlayerScript.ud.Items;
            //Debug.Log("아이템 " + getItem.ItemAmount + "개 들어옴");
        }
        Item item = new Item(getItem);
        if (item.ItemAmount == 0) { Debug.LogError("에러 아이템이 없음"); return 0; }
        var remainItemAmount = item.ItemAmount;
        //스택가능한 아이템인지 체크
        if (item.ItemMaxAmount > 1)
        {
            //스택 가능하면 가능한 공간이 있는지 탐색
            for (var i = 0; i < playerInv.Count; i++)
            {
                if (item.ItemCode == playerInv[i].ItemCode &&
                    playerInv[i].ItemAmount != playerInv[i].ItemMaxAmount)
                {
                    //아이템 코드가 같으면서 스택할 공간이 있는 경우
                    var spaceAmount = playerInv[i].ItemMaxAmount - playerInv[i].ItemAmount;//빈 공간 개수 계산
                    //Debug.Log(i.ToString() + " " + playerInv[i].ItemAmount.ToString()+"합칠 공간의 남은 공간" + spaceAmount.ToString()) ;
                    if (spaceAmount >= remainItemAmount)
                    {
                        playerInv[i].ItemAmount += remainItemAmount;
                        //Debug.Log( "아이템이 완전히 합쳐짐");
                        UIUpdate();
                        if (alert) AlertManager.Instance.AddGameLog(item.ItemName + " " + item.ItemAmount + "개 획득");
                        return 0;
                    }
                    else
                    {
                        Debug.Log("아이템 하나를 합치고 새 아이템을 놓아야함");
                        //아이템 한개를 스택을 가득 채움
                        remainItemAmount -= spaceAmount;
                        playerInv[i].ItemAmount = spaceAmount;
                    }
                }
            }
        }

        //스택 체크 후에도 아이템이 남았을 시 아이템을 빈 칸에 추가함
        for (var i = 0; i < playerInv.Count; i++)
        {
            if (playerInv[i].ItemCode == 0)
            {
                //Debug.Log("빈 공간에 나머지 아이템을 추가해서 넣음");
                var temp = new Item(item);
                if (remainItemAmount > temp.ItemMaxAmount)
                {
                    temp.ItemAmount = item.ItemMaxAmount;
                    remainItemAmount -= temp.ItemMaxAmount;
                }
                else
                {
                    temp.ItemAmount = remainItemAmount;
                    remainItemAmount = 0;
                }
                playerInv[i] = temp;
            }
        }
        UIUpdate();
        if (alert)
        {
            var getAmount = (getItem.ItemAmount - remainItemAmount);
            if (getAmount != 0)
                AlertManager.Instance.AddGameLog(item.ItemName + " " + getAmount.ToString() + "개 획득");
            else
                AlertManager.Instance.AddGameLog("인벤토리 부족");
        }
        return remainItemAmount;
    }
    public bool LoseItem(Item item)
    {
        if (PlayerScript.ud.GetItemAmount(item.ItemCode) >= item.ItemAmount)
        {
            foreach (var i in PlayerItems)
            {
                while (item.ItemAmount > 0 && i.ItemCode == item.ItemCode)
                {
                    item.ItemAmount -= 1;
                    i.ItemAmount -= 1;
                    //아이템 amount가 0이 되면 코드가 0으로 변경되므로 서로 -1만으로 작동 가능
                }
            }
            return true;
        }
        return false;
    }
    public void ShowItemInfo(int a)
    {
        Item targetItem = null;
        if (a >= 0)
            targetItem = PlayerItems[a];
        else if (a == -2)
            targetItem = PlayerScript.ud.Weapon;
        else if (a == -3)
            targetItem = PlayerScript.ud.Armor;
        else if (a == -4)
            targetItem = PlayerScript.ud.Accessory;
        if (targetItem.ItemCode == 0) return;

        Canvas.ForceUpdateCanvases();
        var result = "";
        result += targetItem.ItemName;
        result += "\n\n";
        result += targetItem.ItemDescription;
        if (targetItem.ItemType == ItemType.Armor)
        {
            result += "\n\n";
            result += "방어력 : " + targetItem.ItemPower.ToString();
            if (targetItem.ResistFire > 0)
                result += "\n화염저항 : " + targetItem.ResistFire.ToString();
            if (targetItem.ResistIce > 0)
                result += "\n냉기저항 : " + targetItem.ResistIce.ToString();
            if (targetItem.ResistLight > 0)
                result += "\n전기저항 : " + targetItem.ResistLight.ToString();
            if (targetItem.ResistPoison > 0)
                result += "\n독저항 : " + targetItem.ResistPoison.ToString();
        }
        if (targetItem.ItemType == ItemType.Sword)
        {
            result += "\n\n";
            result += "공격력 : " + targetItem.ItemPower.ToString();
        }
        floatInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = result;
        floatInfo.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)floatInfo.transform.GetChild(0));
    }
    public void HideItemInfo()
    {
        floatInfo.SetActive(false);
    }
    #endregion
    #region UI Update
    public void UIUpdate()
    {
        //Debug.Log(PlayerItems.Count.ToString() + "개의 아이템");
        for (var i = 0; i < PlayerItems.Count; i++)
        {
            if (!(i < inventoryGrid.transform.childCount))
            {
                //인벤토리 자동 확장
                var slot = Instantiate(itemSlot);
                slot.transform.SetParent(inventoryGrid.transform);
                slot.transform.localScale = Vector3.one;
                //slot.GetComponent<InventoryDragger>().windowParent = slot.GetComponent<RectTransform>();
                slot.GetComponent<InventorySlotListener>().canvas = GameManager.Instance.MainUICanvas;
                slot.GetComponent<InventorySlotListener>().myIndex = i;
                invSlots.Add(slot);
            }
            var icon = PlayerItems[i].ItemIcon;
            //아이콘
            inventoryGrid.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = icon;
            //수량
            if (PlayerItems[i].ItemAmount <= 1)
                inventoryGrid.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            else inventoryGrid.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayerItems[i].ItemAmount.ToString();

            if (PlayerItems[i].ItemCode == 0) invSlots[i].GetComponent<InventorySlotListener>().enabled = false;
            else invSlots[i].GetComponent<InventorySlotListener>().enabled = true;
        }
        for (var i = 0; i < 4; i++)
        {
            var icon = PlayerItems[PlayerScript.ud.quickslot[i]].ItemIcon;
            //아이콘
            quickSlot.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = icon;
            //수량
            if (PlayerItems[PlayerScript.ud.quickslot[i]].ItemAmount <= 1)
                quickSlot.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            else quickSlot.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayerItems[PlayerScript.ud.quickslot[i]].ItemAmount.ToString();
        }
        //장비
        eSword.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        if (PlayerScript.ud.Weapon.ItemCode == 0)
        {
            eSword.transform.GetChild(0).GetComponent<Image>().sprite = blankSword;
            eSword.GetComponent<InventorySlotListener>().enabled = false;
        }
        else
        {
            eSword.transform.GetChild(0).GetComponent<Image>().sprite = PlayerScript.ud.Weapon.ItemIcon;
            eSword.GetComponent<InventorySlotListener>().enabled = true;
        }
        eArmor.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        if (PlayerScript.ud.Armor.ItemCode == 0)
        {
            eArmor.transform.GetChild(0).GetComponent<Image>().sprite = blankArmor;
            eArmor.GetComponent<InventorySlotListener>().enabled = false;
        }
        else
        {
            eArmor.transform.GetChild(0).GetComponent<Image>().sprite = PlayerScript.ud.Armor.ItemIcon;
            eArmor.GetComponent<InventorySlotListener>().enabled = true;
        }
        eAccessory.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        if (PlayerScript.ud.Accessory.ItemCode == 0)
        {
            eAccessory.transform.GetChild(0).GetComponent<Image>().sprite = blankAccessory;
            eAccessory.GetComponent<InventorySlotListener>().enabled = false;
        }
        else
        {
            eAccessory.transform.GetChild(0).GetComponent<Image>().sprite = PlayerScript.ud.Accessory.ItemIcon;
            eAccessory.GetComponent<InventorySlotListener>().enabled = true;
        }
        eGold.text = "Gold : " + PlayerScript.ud.Gold.ToString();
    }
    #endregion
}
