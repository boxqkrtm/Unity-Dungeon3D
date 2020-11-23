using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 db 전체를 가지는 객체 싱글턴
//ItemManager.Instance.CodeToItem(itemCode) 으로 아이템 객체 반환

public class QuestManager
{
    static private QuestManager instance = null;
    static public QuestManager Instance
    {
        get
        {
            //Debug.Log("call instance itemmanager");
            if (instance == null)
                instance = new QuestManager();
            return instance;
        }
    }

    private List<Quest> quests;
    public List<Quest> Quest { get { return quests; } }
    public List<Quest> PlayerQuests { get => GameManager.Instance.PlayerScript.ud.Quests; }

    public Quest CodeToQuest(int code)
    {
        return new Quest(quests[code]);
    }

    private QuestManager()
    {
        quests = new List<Quest>();
        var itemCSV = CSVReader.Read("quests");
        foreach (var elem in itemCSV)
        {
            var quest = new Quest(
                int.Parse(elem["id"].ToString()),
                false,
                elem["name"].ToString(),
                elem["description"].ToString(),
                int.Parse(elem["limitLvMin"].ToString()),
                int.Parse(elem["limitLvMax"].ToString()),
                (NpcKind)Enum.Parse(typeof(NpcKind), (elem["npcKind"].ToString())),
                int.Parse(elem["exp"].ToString()),
                int.Parse(elem["itemCode"].ToString()),
                int.Parse(elem["itemAmount"].ToString()),
                (QuestType)Enum.Parse(typeof(QuestType), elem["questType"].ToString()),
                int.Parse(elem["questAmount"].ToString()),
                (NpcKind)Enum.Parse(typeof(NpcKind), elem["questNpc"].ToString()),
                int.Parse(elem["questItemId"].ToString()),
                int.Parse(elem["gold"].ToString())
                );
            quests.Add(quest);
        }
    }

    public List<Quest> GetAvailableQuests(NpcKind thisNpc)
    {
        var quests = QuestManager.Instance.Quest;
        var playerLv = GameManager.Instance.PlayerObj.GetComponent<Player>().ud.Lv;
        var result = new List<Quest>();
        for (var i = 0; i < quests.Count; i++)
        {
            //레벨 조건 및 npc 조건 
            if (playerLv >= quests[i].LimitLvMin && playerLv <= quests[i].LimitLvMax && quests[i].NpcKind == thisNpc)
            {
                if (QuestManager.Instance.PlayerHasQuest(quests[i]) == QuestState.Ended)
                    continue;//종료된 퀘스트는 사용 가능한 퀘스트 목록에서 제외
                result.Add(QuestManager.Instance.CodeToQuest(i));
            }
        }
        return result;
    }

    public string GetQuestInfoStr(Quest q)
    {
        var result = "";
        result = q.Name;
        result += " - 보상 : ";
        result += ItemManager.Instance.CodeToItem(q.ItemCode).ItemName;
        result += " " + q.ItemAmount.ToString() + "개";
        return result;
    }
    public string GetQuestCompleteInfoStr(Quest q)
    {
        var result = "";
        result += ItemManager.Instance.CodeToItem(q.ItemCode).ItemName;
        result += " " + q.ItemAmount.ToString() + "개" + "\n";
        if (q.Exp > 0)
            result += "경험치 " + q.Exp.ToString() + "\n";
        if (q.Gold > 0)
            result += "Gold " + q.Gold.ToString() + "\n";
        result += "을 얻었다.";
        return result;
    }

    public QuestState PlayerHasQuest(Quest quest)
    {
        return GameManager.Instance.PlayerObj.GetComponent<Player>().ud
        .HasQuest(quest);
    }

    public void PlayerClearQuest(Quest quest)
    {
        GameManager.Instance.PlayerObj.GetComponent<Player>().ud.ClearQuest(quest);
    }

    public void PlayerAddQuest(Quest quest)
    {
        GameManager.Instance.PlayerObj.GetComponent<Player>().ud.AddQuest(quest);
    }
}
