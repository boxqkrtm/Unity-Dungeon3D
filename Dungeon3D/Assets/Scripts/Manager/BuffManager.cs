using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    //link
    public Canvas targetCanvas;
    public GameObject buffInfoTextBox;
    public GameObject buffZone;
    public GameObject buffSlot;

    //icon
    public Sprite fireIcon;
    public Sprite dashPotionIcon;
    public Sprite respawnBuffIcon;

    //instance
    private static BuffManager instance = null;
    public static BuffManager Instance
    { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

    //버프
    List<GameObject> buffsGUI = new List<GameObject>();
    public void UpdateBuffGUI(List<Buff> buffs)
    {
        //TODO 호출이 안됨 수정
        //Debug.Log("로드");
        if(buffs.Count != buffsGUI.Count)
        {
            CloseBuffInfo();
            //Debug.Log("버프 전체 삭제");
            for(var i = 0; i< buffsGUI.Count; i++)
            {
                Destroy(buffsGUI[i].gameObject);
                buffsGUI.RemoveAt(i--);
            }
        }
        for(var i =0; i< buffs.Count; i++)
        {
            //Debug.Log("버프 갱신");
            if(i>=buffsGUI.Count)
            {
                //Debug.Log("버프 추가됨");
                GameObject bfg = Instantiate(buffSlot, Vector2.zero, Quaternion.identity);
                bfg.transform.SetParent(buffZone.transform);
                bfg.transform.localScale = new Vector3(1, 1, 1);
                bfg.GetComponent<Image>().sprite = buffs[i]._BuffIcon;
                buffsGUI.Add(bfg);
                buffsGUI[i].GetComponent<BuffSlotListener>().infoTxt = buffs[i].BuffInfo;
            }
            //if (buffsGUI[i] != null) Debug.Log("not null");
            buffsGUI[i].GetComponent<Image>().fillAmount = buffs[i].BuffDurationRatio;
        }
    }

    public void OpenBuffInfo(string v, Vector2 pos)
    {
        buffInfoTextBox.SetActive(true);
        Vector2 pos2 = buffInfoTextBox.transform.position;
        pos2.x = pos.x + 320f * targetCanvas.scaleFactor;
        buffInfoTextBox.transform.position = pos2;
        buffInfoTextBox.transform.GetChild(0).GetComponent<Text>().text = v;
    }
    public void OpenBuffInfoMove(string v, Vector2 pos)
    {
        buffInfoTextBox.SetActive(true);
        Vector2 pos2 = buffInfoTextBox.transform.position;
        pos2.x = pos.x + 190f ;
        buffInfoTextBox.transform.position = pos2;
        buffInfoTextBox.transform.GetChild(0).GetComponent<Text>().text = v;
    }
    public void CloseBuffInfo()
    {
        buffInfoTextBox.SetActive(false);
    }

    public Sprite GetBuffIcon(BuffIcon bi)
    {
        switch (bi)
        {
            case BuffIcon.FireIcon:
                return fireIcon;
            case BuffIcon.DashPotionIcon:
                return dashPotionIcon;
            case BuffIcon.RespawnBuffIcon:
                return respawnBuffIcon;
        }
        return null;
    }

}

public enum BuffIcon
{
    FireIcon, DashPotionIcon, RespawnBuffIcon
}
