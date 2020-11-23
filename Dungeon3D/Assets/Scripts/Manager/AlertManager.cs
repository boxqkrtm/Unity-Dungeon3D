using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//UI의 알림들 담당
public class AlertManager : MonoBehaviour
{
    public GameLog gameLog;
    public GameObject zoneAlert;
    public GameObject levelupAlert;
    //quest
    public GameObject questWindow;
    public GameObject questSlot;

    //delay
    private float zoneDelay = 2;
    private float zoneDelayTimer;
    private float levelupDelay = 2.6f;
    private float levelupDelayTimer;
    //instance
    private static AlertManager instance = null;
    public static AlertManager Instance
    { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(DelayTimer());
        StartCoroutine(UpdateQuestWindow());
    }

    GameObjectList questUI = new GameObjectList();
    IEnumerator UpdateQuestWindow()
    {
        while (true)
        {

            yield return null;
            var ps = GameManager.Instance.PlayerObj.GetComponent<Player>();
            var onGoingQuest = new List<Quest>();
            //진행중인 퀘스트만 받아옴
            foreach (var quest in ps.ud.Quests)
            {
                if (quest.IsQuestComplete == false)
                    onGoingQuest.Add(quest);
            }
            if (onGoingQuest.Count > 0)
            {
                questWindow.SetActive(true);
            }
            else
            {
                questWindow.SetActive(false);
            }

            if (questUI.Count > onGoingQuest.Count)
                for (var i = 0; i < questUI.Count - onGoingQuest.Count; i++)
                    questUI.RemoveAt(questUI.Count - 1);
            else if (questUI.Count < onGoingQuest.Count)
                for (var i = 0; i < onGoingQuest.Count - questUI.Count; i++)
                {
                    var obj = Instantiate(questSlot);
                    obj.transform.SetParent(questWindow.transform);
                    obj.transform.localScale = Vector3.one;
                    questUI.Add(obj);
                }
            //update
            for (var i = 0; i < questUI.Count; i++)
            {
                GameObject obj = questUI[i];
                obj.transform.GetChild(0).GetComponent<Text>().text = onGoingQuest[i].Name; //퀘스트 요약
                obj.transform.GetChild(1).GetComponent<Text>().text = onGoingQuest[i].QuestConditionProgressString;//퀘스트 진행도
            }
        }
    }

    IEnumerator DelayTimer()
    {
        while (true)
        {
            yield return null;
            Timer.Update(ref zoneDelayTimer);
            Timer.Update(ref levelupDelayTimer);
        }
    }

    //큰 토스트 알림
    public void AlertNowArea(string areaName, Vector2Int safeLevel)
    {
        zoneAlert.gameObject.SetActive(true);
        zoneAlert.transform.GetChild(0).GetComponent<Text>().text = areaName;
        zoneAlert.transform.GetChild(1).GetComponent<Text>().text = "권장 레벨 : " + safeLevel.x.ToString() + "~" + safeLevel.y.ToString();
        if (zoneDelayTimer <= 0)
        {
            StartCoroutine(ZoneAlertAnim());
            zoneDelayTimer = zoneDelay;
        }
    }

    public void AlertLevelup()
    {
        var now = GameManager.Instance.PlayerObj.GetComponent<Player>().Lv;
        var bef = now - 1;
        levelupAlert.gameObject.SetActive(true);
        levelupAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = bef.ToString();
        levelupAlert.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = now.ToString();
        if (levelupDelayTimer <= 0)
        {
            StartCoroutine(AlertLevelupAnim());
            levelupDelayTimer = levelupDelay;
        }
    }

    IEnumerator AlertLevelupAnim()
    {
        zoneAlert.GetComponent<Image>().color = new Color(0, 0, 0, 0f);
        //페이드 인
        var deltaTime = 0f;
        while (deltaTime < 0.1f)
        {
            yield return null;
            deltaTime += Time.deltaTime * 1 / Time.timeScale;
            var i = deltaTime * 100;
            levelupAlert.GetComponent<Image>().color = new Color(1, 1, 1, 0.8f * i / 10);
            levelupAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0.6415094f, 0.6415094f, 0.6415094f, 1f * i / 10);
            levelupAlert.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(0.937255f, 0.5411765f, 0.07450981f, 1f * i / 10);
        }
        //대기
        deltaTime = 0f;
        while (deltaTime < 2f)
        {
            yield return null;
            deltaTime += Time.deltaTime * 1 / Time.timeScale;
        }
        //페이드 아웃
        deltaTime = 0f;
        while (deltaTime < 0.5f)
        {
            yield return null;
            deltaTime += Time.deltaTime * 1 / Time.timeScale;
            var i = deltaTime * 100;
            levelupAlert.GetComponent<Image>().color = new Color(1, 1, 1, 0.8f - 0.8f * i / 50);
            levelupAlert.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0.6415094f, 0.6415094f, 0.6415094f, 1f - 1f * i / 50);
            levelupAlert.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(0.937255f, 0.5411765f, 0.07450981f, 1f - 1f * i / 50);
        }
        zoneAlert.gameObject.SetActive(false);

    }

    IEnumerator ZoneAlertAnim()
    {
        zoneAlert.GetComponent<Image>().color = new Color(0, 0, 0, 0f);
        //페이드 인
        var deltaTime = 0f;
        while (deltaTime < 0.5f)
        {
            yield return null;
            deltaTime += Time.deltaTime * 1 / Time.timeScale;
            var i = deltaTime * 100;
            zoneAlert.GetComponent<Image>().color = new Color(1, 1, 1, 0.8f * i / 50);
            zoneAlert.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f * i / 50);
            zoneAlert.transform.GetChild(1).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f * i / 50);
        }
        deltaTime = 0f;
        while (deltaTime < 1f)
        {
            yield return null;
            deltaTime += Time.deltaTime * 1 / Time.timeScale;
        }
        //페이드 아웃
        deltaTime = 0f;
        while (deltaTime < 0.5f)
        {
            yield return null;
            deltaTime += Time.deltaTime * 1 / Time.timeScale;
            var i = deltaTime * 100;
            zoneAlert.GetComponent<Image>().color = new Color(1, 1, 1, 0.8f - 0.8f * i / 50);
            zoneAlert.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f - 1f * i / 50);
            zoneAlert.transform.GetChild(1).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f - 1f * i / 50);
        }
        zoneAlert.gameObject.SetActive(false);
    }

    public void AddGameLog(string str)
    {
        gameLog.Add(str);
    }
}
