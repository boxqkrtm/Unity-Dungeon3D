using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public Item(int itemCode = 0, Sprite itemIcon = null,
        string itemName = "", string itemDescription = "", ItemType itemType = ItemType.Null,
        float itemPower = 0, int resistFire = 0,
        int resistIce = 0, int resistLight = 0, int resistPoison = 0, int itemAmount = 1, int itemMaxAmount = 1,
        int itemPrice = 0)
    {
        ItemCode = itemCode;
        ItemIcon = itemIcon;
        ItemName = itemName;
        ItemDescription = itemDescription;
        ItemType = itemType;
        ItemPower = itemPower;
        ResistFire = resistFire;
        ResistIce = resistIce;
        ResistLight = resistLight;
        ResistPoison = resistPoison;
        ItemAmount = itemAmount;
        ItemMaxAmount = itemMaxAmount;
        ItemPrice = itemPrice;
    }

    public Item(Item item)
    {
        ItemCode = item.ItemCode;
        itemIcon = item.itemIcon; // 따로 저장으로 소문자
        ItemName = item.ItemName;
        ItemDescription = item.ItemDescription;
        ItemType = item.ItemType;
        ItemPower = item.ItemPower;
        ResistFire = item.ResistFire;
        ResistIce = item.ResistIce;
        ResistLight = item.ResistLight;
        ResistPoison = item.ResistPoison;
        ItemAmount = item.ItemAmount;
        ItemMaxAmount = item.ItemMaxAmount;
        ItemPrice = item.ItemPrice;
    }
    public int ItemPrice { get; set; }
    int itemCode;
    public int ItemCode { get { return itemCode; } set { itemCode = value; } }
    [System.NonSerialized]
    public Sprite itemIcon = null;
    public Sprite ItemIcon
    {
        //아이콘은 스프라이트 생성 비용이 크고 Serialized 안되므로 가져옴
        get
        {
            var icon = ItemManager.Instance.CodeToItem(ItemCode).itemIcon;
            if (icon == null)
            {
                return InventoryManager.Instance.blank;
            }
            else return icon;
        }
        set { itemIcon = value; }
    }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public ItemType ItemType { get; set; }
    public float ItemPower { get; set; }//회복량, 공격력, 방어력
    public int ResistFire { get; set; }//각종 저항은 방어구나 장신구에 붙음
    public int ResistIce { get; set; }
    public int ResistLight { get; set; }
    public int ResistPoison { get; set; }
    public int ItemMaxAmount { get; set; }
    int itemAmount;
    public int ItemAmount
    {
        get { return itemAmount; }
        set
        {
            itemAmount = value;
            if (value <= 0)
            {
                ResetSelf();
            }
        }
    }
    public bool Use(UnitData ud)
    {
        switch (ItemType)
        {
            case ItemType.Special:
                AlertManager.Instance.AddGameLog("이 아이템은 사용하는 것이 아니다.");
                return false;
            case ItemType.HealPotion:
                if (ud.HpRatio == 1f)
                {
                    AlertManager.Instance.AddGameLog(
                        "Hp가 이미 가득 차있다.");
                    return false;
                }
                else
                {
                    if (ud.Hp == 0) return false;
                    AlertManager.Instance.AddGameLog(
                        "Hp회복 포션으로 " + ItemPower.ToString() + "회복했다.");
                    ud.Hp += Mathf.RoundToInt(ItemPower);
                    ItemAmount -= 1;
                    return true;
                }
            case ItemType.HealMpPotion:
                if (ud.MpRatio == 1f)
                {
                    AlertManager.Instance.AddGameLog(
                        "Mp가 이미 가득 차있다.");
                    return false;
                }
                else
                {
                    AlertManager.Instance.AddGameLog(
                        "Mp회복 포션으로 " + ItemPower.ToString() + "회복했다.");
                    ud.Mp += Mathf.RoundToInt(ItemPower);
                    ItemAmount -= 1;
                    return true;
                }
            case ItemType.FireStatePotion:
                if (ud.RemoveBuffType(AttackType.Fire) == true)
                {
                    AlertManager.Instance.AddGameLog(
                        "포션으로 화상을 치유했다."
                    );
                    ItemAmount -= 1;
                    return true;
                }
                else
                {
                    AlertManager.Instance.AddGameLog(
                        "해당 상태이상에 걸리지 않았다."
                    );
                    return false;
                }
            case ItemType.IceStatePotion:
                if (ud.RemoveBuffType(AttackType.Ice) == true)
                {
                    AlertManager.Instance.AddGameLog(
                        "포션으로 냉기를 치유했다."
                    );
                    ItemAmount -= 1;
                    return true;
                }
                else
                {
                    AlertManager.Instance.AddGameLog(
                        "해당 상태이상에 걸리지 않았다."
                    );
                    return false;
                }
            case ItemType.PoisonStatePotion:
                if (ud.RemoveBuffType(AttackType.Poison) == true)
                {
                    AlertManager.Instance.AddGameLog(
                        "포션으로 독을 치유했다."
                    );
                    ItemAmount -= 1;
                    return true;
                }
                else
                {
                    AlertManager.Instance.AddGameLog(
                        "해당 상태이상에 걸리지 않았다."
                    );
                    return false;
                }
            case ItemType.SpeedBuffPotion:
                //TODO 구현 버프포션
                AlertManager.Instance.AddGameLog(
                    "몸이 더 가벼워졌다"
                );
                var buff = new Buff();
                buff.SetBuffIcon(BuffIcon.DashPotionIcon);
                buff.BuffName = "신속";
                buff.BuffDuration = 30f;
                buff.BuffPower = ItemPower;
                buff.BuffType = BuffType.Speed;
                buff.BuffResistType = AttackType.None;
                ud.Buffs.Add(buff);
                ItemAmount -= 1;
                return false;
        }
        return false;
    }
    public bool Drop()
    {
        ResetSelf();
        return false;
    }

    public void ResetSelf()
    {
        itemCode = 0;
        itemIcon = null;
        itemAmount = 0;
    }

}
