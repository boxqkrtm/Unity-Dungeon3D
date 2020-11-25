using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NpcKind
{
    DogNpc
}

public enum QuestType
{
    ItemCollect, KillSkeleton
}

[System.Serializable]
public class Quest
{
    #region Constructor
    public Quest(Quest q)
    {
        id = q.Id;
        isQuestComplete = q.IsQuestComplete;
        name = q.Name;
        description = q.Description;
        limitLvMin = q.LimitLvMin;
        limitLvMax = q.LimitLvMax;
        npcKind = q.NpcKind;
        exp = q.Exp;
        itemCode = q.ItemCode;
        itemAmount = q.ItemAmount;
        questItemId = q.QuestItemId;
        questType = q.QuestType;
        questAmount = q.QuestAmount;
        questNpc = q.QuestNpc;
        gold = q.Gold;
        progress = q.ProgressValue;
    }

    public Quest(int id, bool isQuestComplete, string name,
    string description
    , int limitLvMin, int limitLvMax, NpcKind npcKind, int exp,
    int itemcode, int itemAmount, QuestType questType,
    int questAmount, NpcKind questNpc, int questItemId,
    int gold)
    {
        this.id = id;
        this.isQuestComplete = isQuestComplete;
        this.name = name;
        this.description = description;
        this.limitLvMin = limitLvMin;
        this.limitLvMax = limitLvMax;
        this.npcKind = npcKind;
        this.exp = exp;
        this.itemCode = itemcode;
        this.itemAmount = itemAmount;
        this.questType = questType;
        this.questAmount = questAmount;
        this.questNpc = questNpc;
        this.questItemId = questItemId;
        this.gold = gold;
        this.progress = 0;
    }
    #endregion
    #region Quest Info
    [SerializeField]
    int id;
    public int Id { get { return id; } set { id = value; } }
    [SerializeField]
    string name;
    public string Name { get { return name; } set { name = value; } }
    [SerializeField]
    string description;
    public string Description { get { return description; } set { description = value; } }
    #endregion
    #region Quest Limits 퀘스트를 받을 수 있는 제한
    [SerializeField]
    int limitLvMin;
    public int LimitLvMin { get { return limitLvMin; } set { limitLvMin = value; } }
    [SerializeField]
    int limitLvMax;
    public int LimitLvMax { get { return limitLvMax; } set { limitLvMax = value; } }
    [SerializeField]
    NpcKind npcKind;
    public NpcKind NpcKind { get { return npcKind; } set { npcKind = value; } }
    #endregion
    #region Quest Complete Conditions
    [SerializeField]
    int gold;
    public int Gold { get { return gold; } set { gold = value; } }
    [SerializeField]
    QuestType questType;
    public QuestType QuestType { get { return questType; } set { questType = value; } }
    [SerializeField]
    int questAmount;//클리어 횟수, 아이템 개수 등
    public int QuestAmount { get { return questAmount; } set { questAmount = value; } }
    [SerializeField]
    int questItemId;
    public int QuestItemId { get => questItemId; set => questItemId = value; }
    [SerializeField]
    NpcKind questNpc;//도착 npc
    public NpcKind QuestNpc { get { return questNpc; } set { questNpc = value; } }
    int progress;//토벌 횟수 등을 저장하는 변수
    public int ProgressValue { get => progress; set => progress = value; }
    #endregion
    #region Quest Rewards
    [SerializeField]
    int exp;
    public int Exp { get { return exp; } set { exp = value; } }
    [SerializeField]
    int itemCode;
    public int ItemCode { get { return itemCode; } set { itemCode = value; } }
    [SerializeField]
    int itemAmount;
    public int ItemAmount { get { return itemAmount; } set { itemAmount = value; } }
    #endregion
    #region Quest State
    [SerializeField]
    bool isQuestComplete;
    public bool IsQuestComplete { get { return isQuestComplete; } set { isQuestComplete = value; } } //완료된 퀘스트 체크
    private bool IsQuestConditionComplete
    {
        get
        {
            switch (QuestType)
            {
                case QuestType.ItemCollect:
                    if (GameManager.Instance.PlayerObj.GetComponent<Player>()
                    .ud.GetItemAmount(QuestItemId) >= QuestAmount)
                        return true;
                    else return false;
                case QuestType.KillSkeleton:
                    return ProgressValue >= QuestAmount ? true : false;
            }
            return true;
        }
    }
    public QuestState QuestState
    {
        get
        {
            if (IsQuestComplete)
                return QuestState.Ended;
            else if (IsQuestConditionComplete)
                return QuestState.Completed;
            else return QuestState.Ongoing;
        }
    }
    //퀘스트 진행도를 문자로 나타내주는 함수
    public string QuestConditionProgressString
    {
        get
        {
            var result = "";
            //Debug.Log(QuestType.ToString());
            switch (QuestType)
            {
                case QuestType.ItemCollect:
                    result += GameManager.Instance.PlayerObj.GetComponent<Player>()
                    .ud.GetItemAmount(QuestItemId).ToString();
                    result += "/" + QuestAmount.ToString();
                    result += IsQuestConditionComplete == true ? "(완료)" : "";
                    return result;
                case QuestType.KillSkeleton:
                    result = ProgressValue.ToString() + "/" + QuestAmount.ToString();
                    result += IsQuestConditionComplete == true ? "(완료)" : "";
                    return result;
            }
            return "";
        }
    }
    #endregion
    #region Quest Execution Function
    public void ClearQuest()
    {
        var ps = GameManager.Instance.PlayerObj.GetComponent<Player>();
        //퀘스트 조건 지불
        switch (QuestType)
        {
            case QuestType.ItemCollect:
                var item = ItemManager.Instance.CodeToItem(QuestItemId);
                item.ItemAmount = QuestAmount;
                InventoryManager.Instance.LoseItem(item);
                break;
        }
        //아이템 지급
        if (ItemCode != 0)
        {
            var item = ItemManager.Instance.CodeToItem(ItemCode);
            item.ItemAmount = this.ItemAmount;
            EffectManager.Instance.CreateDroppedItem(GameManager.Instance.PlayerObj.transform.position, item);
        }
        //경험치 지급
        ps.ud.Exp += Exp;
        //돈 지급
        ps.ud.Gold += Gold;
        //퀘스트 완료처리
        IsQuestComplete = true;
    }
    #endregion

}