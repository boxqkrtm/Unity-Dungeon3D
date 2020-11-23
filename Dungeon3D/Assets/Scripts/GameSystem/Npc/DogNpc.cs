using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DogNpc : Npc
{
    override protected int NpcData
    {
        get { return gmgr.Nd.DogNpcStory; }
        set { gmgr.Nd.DogNpcStory = value; }
    }
    override protected void TalkingCustomFunction(int state)
    {
        var quests = QuestManager.Instance.GetAvailableQuests(NpcKind.DogNpc);
        if (state == -2)//퀘스트 목록
        {
            //퀘스트의 목록 칸을 채워줌
            talks[4].mainText = "- 퀘스트 목록 -";
            talks[4].mainText += "\n\n";
            for (int i = 0; i < quests.Count; i++)
            {
                var str = "";
                str += (i + 1).ToString() + ". "
                + QuestManager.Instance.GetQuestInfoStr(quests[i]);
                switch (QuestManager.Instance.PlayerHasQuest(quests[i]))
                {
                    case QuestState.None:
                        str += "";
                        break;
                    case QuestState.Ongoing:
                        str += " (진행중) ";
                        break;
                    case QuestState.Completed:
                        str += " (완료) ";
                        break;
                }
                talks[4].mainText += str + "\n";
            }
            talks[4].options.Clear();
            for (int i = 0; i < quests.Count; i++)
            {
                if (QuestManager.Instance.PlayerHasQuest(quests[i]) == QuestState.None) // 퀘스트가 없을 때
                {
                    talks[4].options.Add(
                        new NpcTalkOption((i + 1).ToString(), -3 - i, false));//-3456
                }
                else if (QuestManager.Instance.PlayerHasQuest(quests[i]) == QuestState.Completed) //퀘스트 조건 충족
                {
                    talks[4].options.Add(
                        new NpcTalkOption((i + 1).ToString(), -7 - i, false));//-78910
                }
                else if (QuestManager.Instance.PlayerHasQuest(quests[i]) == QuestState.Ongoing) //퀘스트 진행중
                {
                    talks[4].options.Add(
                        new NpcTalkOption((i + 1).ToString(), 6, false));
                }
            }
            talks[4].options.Add(new NpcTalkOption("그만두기", 2, true));

            //생성한 목록으로 대사 이동
            tempNpcData = 4;
        }
        else if (state <= -3 && state >= -6)//퀘스트 받기 처리
        {
            var selectedQuest = quests[(state * -1) - 3];
            //퀘스트 받기 처리
            QuestManager.Instance.PlayerAddQuest(selectedQuest);

            //기본 대사로 대사 이동
            tempNpcData = 2;
        }
        else if (state <= -7 && state >= -10)//퀘스트 완료 처리
        {
            var selectedQuest = quests[(state * -1) - 7];
            //퀘스트 받기 처리
            tempNpcData = 5;
            talks[5].mainText = "다 구해왔구나 자 여기\n";
            talks[5].mainText += QuestManager.Instance.GetQuestCompleteInfoStr(selectedQuest);
            QuestManager.Instance.PlayerClearQuest(selectedQuest);
            if (selectedQuest.Id == 1)
            {
                AlertManager.Instance.AddGameLog("운송수단이 복구되어 상점을 이용 할 수 있게 되었다.");
            }
        }
        else if (state == -12) //상점
        {
            //상점
            if (QuestManager.Instance.PlayerHasQuest(QuestManager.Instance.CodeToQuest(1)) == QuestState.Ended)
            {
                var list = new List<Item>();
                var codes = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                foreach (var itemCode in codes)
                    list.Add(ItemManager.Instance.CodeToItem(itemCode));
                NpcManager.Instance.OpenShopWindow(list);
                tempNpcData = 7;
            }
            else
                tempNpcData = 3; //코드 1퀘스트 미 완료시 상점 이용불가
        }
        else if (state == -13)
        {
            NpcManager.Instance.CloseShopWindow();
            tempNpcData = 2;
        }
    }
}
