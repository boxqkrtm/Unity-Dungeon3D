using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 전반적인 게임 오브젝트와 게임 데이터를 관리하는 클래스
/// </summary>

[System.Serializable]
public class GameData
{
    public UnitData ud;
    public NpcData nd;
}
[System.Serializable]
public class NpcData
{
    int dogNpcStory = 0;
    public int DogNpcStory { get { return dogNpcStory; } set { dogNpcStory = value; } }
}

public class GameManager : MonoBehaviour
{
    #region variable
    public Vector3 defaultLocation;
    public static bool DEBUG = false;
    NpcData nd;
    public NpcData Nd { get { return nd; } set { nd = value; } }
    public GameObject gameoverUI;
    GameObject player;
    public GameObject PlayerObj { get { if (player == null) player = GameObject.FindGameObjectWithTag("Player"); return player; } }
    public Player PlayerScript { get => PlayerObj.GetComponent<Player>(); }
    //skill
    public GameObject skillWindow;
    //instance
    private static GameManager instance = null;
    public static GameManager Instance
    { get { return instance; } }

    public Canvas mainUICanvas;
    public Canvas MainUICanvas { get { return mainUICanvas; } }

    private void Awake()
    {
        instance = this;
    }

    string gameSavePath;
    #endregion
    private void Start()
    {
        nd = new NpcData();
        gameoverUI.SetActive(true);
        GameObject.Find("RespawnButton").GetComponent<Button>().onClick.AddListener(
        () => { RespawnGame(true); gameoverUI.SetActive(false); });
        GameObject.Find("GoldRespawnButton").GetComponent<Button>().onClick.AddListener(
            () => { RespawnGame(false); gameoverUI.SetActive(false); });
        //GameObject.Find("DataResetButton").GetComponent<Button>().onClick.AddListener(
        //    () => { ResetGame(); gameoverUI.SetActive(false); });
        gameoverUI.SetActive(false);

        gameSavePath = Application.persistentDataPath + "/save.dat";
        if (LoadGame() == false)
        {
            //데이터 없음 새로 생성
            PlayerObj.GetComponent<Player>().ud = new UnitData(false);
            PlayerObj.GetComponent<Player>().InitUd();
        }
        else
        {
            //데이터 로드 성공
            PlayerObj.transform.position = PlayerScript.ud.Location.ToVector3();
        }
        AutoSaveRoutine = StartCoroutine(AutoSave());
        PlayerScript.ud.Location = new Vector3Data(PlayerObj.transform.position);
        PlayerScript.ud.SceneName = SceneManager.GetActiveScene().name;
    }

    #region Save/Load
    Coroutine AutoSaveRoutine;
    IEnumerator AutoSave()
    {
        var delay = 0f;
        while (true)
        {
            yield return null;
            delay += Time.deltaTime * (1 / Time.timeScale);
            if (delay >= 1f)
            {
                PlayerScript.ud.Location = new Vector3Data(PlayerObj.transform.position);
                SaveGame();
                delay = 0f;
            }
        }
    }
    public void StopAutoSave()
    {
        StopCoroutine(AutoSaveRoutine);
    }
    public void StartAutoSave()
    {
        if (AutoSaveRoutine == null)
            AutoSaveRoutine = StartCoroutine(AutoSave());
    }

    IEnumerator InitDebug()
    {
        var r = true;
        while (r)
        {
            if (player != null)
            {
                Debug.Log("init succeed");
                for (var i = 0; i < 10; i++)
                    PlayerObj.GetComponent<Player>().ud.AddItem(new Item());
                Debug.Log(PlayerObj.GetComponent<Player>().ud.Items.Count);
                r = false;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    //게임데이터
    public void SaveGame()
    {
        var data = new GameData();
        data.ud = PlayerObj.GetComponent<Player>().ud;
        data.nd = nd;
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(gameSavePath))
        {
            binaryFormatter.Serialize(fileStream, (GameData)data);
        }
    }

    public bool LoadGame()
    {
        if (File.Exists(gameSavePath))
        {
            try
            {
                var binaryFormatter = new BinaryFormatter();
                using (var fileStream = File.Open(gameSavePath, FileMode.Open))
                {
                    var gd = (GameData)binaryFormatter.Deserialize(fileStream);
                    PlayerObj.GetComponent<Player>().ud = gd.ud;
                    PlayerObj.GetComponent<Player>().InitUd();
                    nd = gd.nd;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    static public GameData LoadGameData(String gameSavePath)
    {
        if (File.Exists(gameSavePath))
        {
            try
            {
                var binaryFormatter = new BinaryFormatter();
                using (var fileStream = File.Open(gameSavePath, FileMode.Open))
                {
                    GameData gd = (GameData)binaryFormatter.Deserialize(fileStream);
                    return gd;
                }
            }
            catch { }
        }
        return null;
    }

    public void ResetGame()
    {
        if (File.Exists(gameSavePath))
        {
            File.Delete(gameSavePath);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    #endregion

    public void RespawnGame(bool isPenelty = true)
    {
        //부활 시킨 후 게임을 저장하고 신을 다시 로드함
        var pscript = PlayerObj.GetComponent<Player>();
        pscript.ud.Buffs.Clear();
        if (isPenelty)
        {
            //사망 부활 패널티
            pscript.ud.HpRatio = 0.3f;
            pscript.ud.MpRatio = 0.3f;
            pscript.ud.Gold = Mathf.RoundToInt((float)pscript.ud.Gold * 0.9f);
            pscript.ud.Exp = Mathf.RoundToInt((float)pscript.ud.Exp * 0.9f);
            pscript.ud.Location = new Vector3Data(defaultLocation);
            SaveGame();
            StopAutoSave();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            pscript.ud.HpRatio = 1f;
            pscript.ud.MpRatio = 1f;
            pscript.ud.Buffs.Add(new Buff("부활의 축복", BuffType.Atk, AttackType.None, BuffIcon.RespawnBuffIcon, (float)pscript.ud.Atk * 0.1f, 30f, ""));
            pscript.ud.Buffs.Add(new Buff("부활의 축복", BuffType.Def, AttackType.None, BuffIcon.RespawnBuffIcon, (float)pscript.ud.Def * 0.1f, 30f, ""));
            pscript.ud.Buffs.Add(new Buff("부활의 축복", BuffType.Hp, AttackType.None, BuffIcon.RespawnBuffIcon, (float)pscript.ud.Hp * 0.1f, 30f, ""));
            pscript.ud.Buffs.Add(new Buff("부활의 축복", BuffType.Mp, AttackType.None, BuffIcon.RespawnBuffIcon, (float)pscript.ud.Mp * 0.1f, 30f, ""));
            pscript.ud.Buffs.Add(new Buff("부활의 축복", BuffType.Speed, AttackType.None, BuffIcon.RespawnBuffIcon, 1.2f, 30f, ""));
            pscript.Respawn();
        }
        SaveGame();
    }
    public void OpenGameoverUI()
    {
        gameoverUI.SetActive(true);
    }
    //플레이어
    public void PlayerSwordHit(GameObject go, Vector3 hitPosition)
    {
        PlayerObj.GetComponent<Player>().PlayerSwordHit(go, hitPosition);
    }
    public void PlayerGetExpByKill(int amount)
    {
        PlayerObj.GetComponent<Player>().GetExp(amount);
    }
    public void PlayerGetGoldByKill(int amount)
    {
        PlayerObj.GetComponent<Player>().ud.Gold += amount;
    }
    public void PlayerGetLevelUp()
    {
        Vector3 pos = player.transform.position;
        pos.y += 0.1f;

        EffectManager.Instance.CreateLevelupEffect(pos);
        AlertManager.Instance.AlertLevelup();
    }
    public void DestroyGameObject(GameObject obj)
    {
        Destroy(obj);
    }

    public void MoveScene(string sceneName, Vector3 location)
    {
        var ps = InputManager.Ps;
        ps.Disable();
        StopAutoSave();
        PlayerScript.ud.Location = new Vector3Data(location);
        PlayerScript.ud.SceneName = sceneName;
        SaveGame();
        SceneManager.LoadScene("LoadingScene");
    }

    public void OpenSkillWindow()
    {
        skillWindow.SetActive(!skillWindow.activeSelf);
    }

    public void UpStairDungeon()
    {
        var dg = GameObject.Find("DungeonGenerator").GetComponent<MapGenerator>();
        dg.floor += 1;
        dg.GenerateFloor();

    }

}
