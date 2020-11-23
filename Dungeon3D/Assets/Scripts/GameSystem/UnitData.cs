using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Vector3Data
{
    float x, y, z;
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public Vector3Data(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
}

/// <summary>
/// 스탯 아이템 상태이상 현재상태 및 상태 계산식을 담당하는 클래스
/// </summary>
[System.Serializable]
public class UnitData
{
    #region location
    string sceneName;
    public string SceneName { get => sceneName; set => sceneName = value; }
    Vector3Data location;
    public Vector3Data Location { get => location; set => location = value; }
    #endregion
    #region stat constant
    //능력치 레벨 등 최댓값 (버프, 장비 제외)
    const int MAX_LEVEL = 99; //최대 레벨 일 때 = 최대 공격력
    const int MAX_ATK = 500;
    const int MAX_DEF = 250;
    const int MAX_HP = 10000;
    const int MAX_MP = 10000;
    const int MAX_NATURALHEAL = 100;
    #endregion
    #region stat linear function
    private int LvToStat(int level, int max)
    {
        return Mathf.RoundToInt(((float)level / MAX_LEVEL) * max);
    }
    public int GetMaxExp(int lv)
    {
        //요구 경험치 공식
        return Mathf.RoundToInt(Mathf.Pow((float)lv * 50 / 49, 2.5f));
    }
    #endregion
    #region stat changeListener
    private void DelePlaceholder() { }
    public delegate void OnHpChanged();
    public delegate void OnMpChanged();
    public delegate void OnExpChanged();
    public delegate void OnLvChanged();
    [System.NonSerialized]
    public OnHpChanged onHpChanged;
    [System.NonSerialized]
    public OnMpChanged onMpChanged;
    [System.NonSerialized]
    public OnExpChanged onExpChanged;
    [System.NonSerialized]
    public OnLvChanged onLvChanged;
    [SerializeField]
    #endregion
    #region damage calculations
    public int TakeDamageCalc(AttackType at, int amount)
    {
        //데미지는 +- 30%의 오차가 발생함
        //최하 1데미지 
        //속성 공격을 받을 때, 통상 방어력은 절반밖에 적용 안됨
        var damage = amount + Mathf.RoundToInt((float)amount * Random.Range(-0.3f, +0.3f));
        damage = DamageResistCalc(at, damage); // 저항 공식에 따라 1차 데미지 산정
        damage = damage > 0 ? damage : 1; // 최하 1 데미지에 따라 최종 데미지 산정
        return damage;
    }

    public int DamageResistCalc(AttackType at, int amount)
    {
        var damage = amount;
        if (at == AttackType.Atk)
        {
            damage -= Def;
        }
        else if (at == AttackType.None)
        {
            //무속성 저항 없음
        }
        else
        //일반 마법속성, 속성저항 및 일반 방어력 절반
        {
            switch (at)
            {
                case AttackType.Fire:
                    damage = damage - ResistFire - Def / 4;
                    break;
                case AttackType.Ice:
                    damage = damage - ResistIce - Def / 4;
                    break;
                case AttackType.Light:
                    damage = damage - ResistLight - Def / 4;
                    break;
                case AttackType.Poison:
                    damage = damage - ResistPoison - Def / 4;
                    break;
            }
        }
        return damage;
    }

    //상태이상 공격 맞음
    public void TakeSpecial(Buff buff, float chance)
    {
        var rand = Random.Range(0f, 1f);
        //적용 확률통과
        if (chance > rand)
        {
            //디버프의 경우 저항 //감속버프는 저항이 아직 없음
            if (buff.BuffPower < 0 && buff.BuffType != BuffType.Speed)
            {
                //저항 검사
                var power = buff.BuffPower;
                //Debug.Log("버프 파워" + power.ToString());
                power = DamageResistCalc(buff.BuffResistType, -Mathf.RoundToInt(buff.BuffPower));
                //Debug.Log("버프 파워 저항" + power.ToString());
                //저항 후 파워가 남으면 버프 적용, 없으면 버프 무시
                if (power > 0)
                {
                    buff.BuffPower = -power;
                    Buffs.Add(buff);
                }
                else if (power <= 0)
                {
                    //디버프 면역
                }
            }
            else Buffs.Add(buff);
            //버프의 경우 그냥 받음
        }
    }

    public float CalculateDamageEffective(int damage, int amount)
    {
        var realDamage = damage + Def;
        return (float)realDamage / amount;
    }
    #endregion
    #region stats
    [SerializeField]
    string name;
    public string Name { get { return name; } set { name = value; } }
    [SerializeField]
    int hp;
    public int Hp
    {
        get { return hp; }
        set
        {
            var befHp = hp;
            hp = value;
            if (hp >= MaxHp) hp = MaxHp;
            if (hp < -1) hp = 0;
            if (befHp != hp) onHpChanged();
        }
    }
    public float HpRatio { get { return (float)Hp / MaxHp; } set { Hp = Mathf.RoundToInt(MaxHp * value); } }
    [SerializeField]
    int mp;
    public int Mp
    {
        get
        {
            return mp;
        }
        set
        {
            var befMp = mp;
            mp = value;
            if (mp >= MaxHp) mp = MaxHp;
            if (mp < -1) mp = 0;
            if (befMp != mp) onMpChanged();
        }
    }
    private float mpFloatMinus;
    public float MpFloatMinus
    {
        get { return mpFloatMinus; }
        set
        {
            mpFloatMinus = value;
            while (mpFloatMinus >= 1f)
            {
                mpFloatMinus -= 1f;
                Mp -= 1;
            }
        }
    }
    public float MpRatio { get { return (float)Mp / MaxMp; } set { Mp = Mathf.RoundToInt(MaxMp * value); } }
    [SerializeField]
    int exp;
    public int Exp
    {
        get { return exp; }
        set
        {
            exp = value;

            while (MaxExp <= exp)
            {
                exp -= MaxExp;
                Lv += 1;
            }
            onExpChanged();
        }
    }
    public float ExpRatio { get { return (float)Exp / MaxExp; } set { Exp = Mathf.RoundToInt(MaxExp * value); } }
    [SerializeField]
    int maxHp;
    public int MaxHp { get { if (IsEnemy == false) maxHp = LvToStat(lv, MAX_HP); return maxHp; } set { maxHp = value; } }
    [SerializeField]
    int maxMp;
    public int MaxMp { get { if (IsEnemy == false) maxMp = LvToStat(lv, MAX_MP); return maxMp; } set { maxMp = value; } }
    public int MaxExp { get { return GetMaxExp(Lv); } } //몬스터의 경우 획득 경험치
    [SerializeField]
    int lv;
    public int Lv { get { return lv; } set { lv = value; onLvChanged(); } }
    [SerializeField]
    int atk;
    public int Atk
    {
        get
        {
            if (IsEnemy == false)
            {
                //레벨 데이터 받아옴
                var result = LvToStat(Lv, MAX_ATK);
                //버프 데이터 받아옴
                result += buffAtk;
                //장비데이터 받아옴
                result += Mathf.RoundToInt(Weapon.ItemPower);
                //총합 계산
                atk = result;//인스펙터 표시용
                return result;
            }
            else return atk + buffAtk;
        }
        set { atk = value; }
    }
    public int PureAtk//버프만 제외
    {
        get
        {
            if (IsEnemy == false)
            {
                //레벨 데이터 받아옴
                var result = LvToStat(Lv, MAX_ATK);
                //장비데이터 받아옴
                result += Mathf.RoundToInt(Weapon.ItemPower);
                //총합 계산
                return result;
            }
            else return atk;
        }
    }
    [SerializeField]
    int def;
    public int Def
    {
        get
        {
            if (IsEnemy == false)
            {
                //레벨 데이터 받아옴
                var result = LvToStat(Lv, MAX_DEF);
                //버프 데이터 받아옴
                result += buffDef;
                //장비데이터 받아옴
                result += Mathf.RoundToInt(Armor.ItemPower);
                //총합 계산
                def = result;
                return result;
            }
            else return def + buffDef;
        }
        set { def = value; }
    }
    [SerializeField]
    int resistFire;
    [SerializeField]
    int resistIce;
    [SerializeField]
    int resistLight;
    [SerializeField]
    int resistPoison;
    public int ResistFire { get { if (IsEnemy == false) resistFire = Armor.ResistFire; return resistFire; } }
    public int ResistIce { get { if (isEnemy == false) resistIce = Armor.ResistIce; return resistIce; } }
    public int ResistLight { get { if (isEnemy == false) resistLight = Armor.ResistLight; return resistLight; } }
    public int ResistPoison { get { if (isEnemy == false) resistPoison = Armor.ResistPoison; return resistPoison; } }
    [SerializeField]
    int naturalHeal;
    public int NaturalHeal { get { if (IsEnemy == false) naturalHeal = LvToStat(Lv, MAX_NATURALHEAL); return naturalHeal; } set { naturalHeal = value; } } //5초마다 회복되는 체력의 양
    [SerializeField]
    float speed;
    public float Speed
    {
        get
        {
            //플레이어는 기본속도 6.5f 고정, 버프로만 상승 가능
            if (IsEnemy == false) return 6.5f * buffSpeed;
            else return speed * buffSpeed;
        }
        set { speed = value; }
    }
    [SerializeField]
    float acc;
    public float Acc { get { return acc; } set { acc = value; } } //공격 명중률
    [SerializeField]
    float avd;
    public float Avd { get { return avd; } set { avd = value; } } //공격 회피율
    [SerializeField]
    bool isEnemy;
    bool IsEnemy { get { return isEnemy; } set { isEnemy = value; } }
    #endregion
    #region inventory
    [SerializeField]
    int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            InventoryManager.Instance.UIUpdate();
            gold = value;
        }
    } //몬스터의 경우 획득 골드
    List<Item> items = new List<Item>();//몬스터의 경우 획득 할 아이템
    [SerializeField]
    List<ItemCodeAmount> itemCodeAmount = new List<ItemCodeAmount>();//인스펙터에서 아이템을 추가할 때 사용
    public List<Item> Items { get { return items; } }
    //인벤토리를 생성시킴 ( 공간 증가 )
    public bool AddItem(Item item)
    {
        items.Add(item);
        return false;
    }
    public int GetItemAmount(int itemCode)
    {
        var result = 0;
        foreach (var i in items)
        {
            if (i.ItemCode == itemCode)
            {
                result += i.ItemAmount;
            }
        }
        return result;
    }
    [SerializeField]
    Item armor;
    public Item Armor { get { return armor; } set { armor = value; } }
    [SerializeField]
    Item accessory;
    public Item Accessory { get { return accessory; } set { accessory = value; } }
    Item weapon;
    public Item Weapon { get { return weapon; } set { weapon = value; } }
    #endregion
    #region buff
    [SerializeField]
    List<Buff> buffs = new List<Buff>();//버프나 디버프
    int buffAtk;
    int buffDef;
    float buffSpeed = 1f;
    public List<Buff> Buffs { get { return buffs; } set { buffs = value; } }
    //버프 연산을 위한 루프
    float naturalHealTimer = 0f;
    public void buffLoop(GameObject my, GameObject buffZone = null)
    {
        if (Hp == 0) return;
        //1초마다 자연회복
        NaturalHealFrameLoop();
        //버프 능력치 합산을 위한 초기화
        buffAtk = 0;
        buffDef = 0;
        buffSpeed = 1f;
        //전체 버프들 검사 및 기능 수행
        AllBuffFrameLoop(my, buffZone);
        buffSpeed = buffSpeed > 2f ? 2f : buffSpeed;
    }

    private void NaturalHealFrameLoop()
    {

        if (naturalHealTimer < 0)
        {
            Hp += NaturalHeal;
            Mp += NaturalHeal;
            naturalHealTimer = 1f;
        }
        else naturalHealTimer -= Time.deltaTime;
    }

    private void AllBuffFrameLoop(GameObject my, GameObject buffZone)
    {
        for (int i = 0; i < Buffs.Count; i++)
        {
            Buffs[i].SubBuffDuration(Time.deltaTime);//버프 시간 감소
            var isDebuff = Buffs[i].BuffPower < 0 ? true : false;
            switch (Buffs[i].BuffType)
            {
                case BuffType.Hp:
                    //초마다 발동하는 버프
                    Buffs[i].BuffDelta += Time.deltaTime;//버프 감소 된 시간 계산
                    if (Buffs[i].BuffDelta >= 1f)//1초가 되면 감소 된 시간 합을 0으로 만들고 버프 역할 수행
                    {
                        Buffs[i].BuffDelta -= 1f;
                        Hp += Mathf.RoundToInt(Buffs[i].BuffPower);
                        //디버프인 경우 상태이상에 따른 피격 이팩트
                        if (isDebuff == true)
                        {
                            if (Buffs[i].BuffResistType == AttackType.Fire)
                                EffectManager.Instance.CreateHitFireEffect(my.transform.position);
                        }
                    }
                    break;
                case BuffType.Mp: //초마다 발동하는 버프
                    Buffs[i].BuffDelta += Time.deltaTime;//버프 감소 된 시간 계산
                    if (Buffs[i].BuffDelta >= 1f)//1초가 되면 감소 된 시간 합을 0으로 만들고 버프 역할 수행
                    {
                        Buffs[i].BuffDelta -= 1f;
                        Mp += Mathf.RoundToInt(Buffs[i].BuffPower);
                    }
                    break;
                case BuffType.Atk:
                    buffAtk += Mathf.RoundToInt(Buffs[i].BuffPower);
                    break;
                case BuffType.Def:
                    buffDef += Mathf.RoundToInt(Buffs[i].BuffPower);
                    break;
                case BuffType.Speed:
                    buffSpeed *= Buffs[i].BuffPower;
                    break;
            }
            if (Buffs[i].BuffDuration <= 0f) Buffs.RemoveAt(i--);
        }
        if (buffZone != null) //버프존이 없으면 업데이트 현재는 플레이어의 버프 상태창만 취급
            BuffManager.Instance.UpdateBuffGUI(Buffs);
    }
    public bool RemoveBuffType(AttackType at)
    {
        bool succeed = false;
        for (var i = 0; i < Buffs.Count; i++)
        {
            if (Buffs[i].BuffResistType == at)
            {
                Buffs.RemoveAt(i);
                succeed = true;
            }
        }
        return succeed;
    }
    #endregion
    #region quest
    [SerializeField]
    List<Quest> quests;
    public List<Quest> Quests { get { return quests; } }
    public void AddQuest(Quest quest)
    {
        quests.Add(quest);
    }

    //return 0: 퀘스트 없음
    //return 1 퀘스트 있음
    //return 2 퀘스트 조건 만족
    //return 3 퀘스트 완료 ()
    public QuestState HasQuest(Quest quest)
    {
        foreach (var i in quests)
            if (i.Id == quest.Id)
                return i.QuestState;
        return QuestState.None;
    }

    public void ClearQuest(Quest q)
    {
        foreach (var i in quests)
        {
            if (i.Id == q.Id)
            {
                i.ClearQuest();
            }
        }
    }

    #endregion
    #region skill

    List<Skill> skills = new List<Skill>();
    public List<Skill> Skills { get => skills; set => skills = value; }
    public void SkillLoop()
    {
        foreach (var i in skills)
        {
            i.SkillDelayLoop();
        }
    }
    #endregion
    #region initializers
    public UnitData(bool isEnemy = true, string name = "",
    int hp = 1, int mp = 1, int exp = 0, int lv = 1
        , int gold = 0, int atk = 0, int def = 0,
    int resistFire = 0, int resistIce = 0, int resistLight = 0,
        int resistPoison = 0, int naturealHeal = 0
        , float speed = 1f, float acc = 1f, float avd = 0f)
    {
        //속성저항은 몬스터 또는 특수 캐릭터만 가지고 있음 레벨업으로 변화하지 않음
        //일반사람은 전부 0 장비로 올림

        //공격력 방어력의 스탯은 몬스터의 경우 그대로 적용
        //플레이어의 경우 set은 의미 없음

        //이동속도 명중률 회피률은 성장하지 않음
        this.name = name;
        maxHp = hp;
        maxMp = mp;
        this.exp = exp;
        this.lv = lv;
        this.gold = gold;
        Atk = atk;
        Def = def;
        this.resistFire = resistFire;
        this.resistIce = resistIce;
        this.resistLight = resistLight;
        this.resistPoison = resistPoison;
        NaturalHeal = naturealHeal;
        Speed = speed;
        Acc = acc;
        Avd = avd;
        IsEnemy = isEnemy;
        UnitDataInit();
    }
    /// <summary>
    /// 생성자를 사용하지 않고 인스펙터 상에서만 입력했을 때
    /// Start에서 Init를 실행해주면 정상동작
    /// </summary>
    public void UnitDataInit()
    {
        onHpChanged = new OnHpChanged(DelePlaceholder);
        onMpChanged = new OnMpChanged(DelePlaceholder);
        onExpChanged = new OnExpChanged(DelePlaceholder);
        onLvChanged = new OnLvChanged(DelePlaceholder);
        hp = MaxHp;
        mp = MaxMp;
        buffAtk = 0;
        buffDef = 0;
        buffSpeed = 1f;
        buffs = new List<Buff>();
        items = new List<Item>();
        quests = new List<Quest>();
        skills = new List<Skill>();
        skills.Add(new Skill("파이널 슬래시", 1, 5, SkillFunction.Skill1, 5f, 50));
        if (isEnemy == false && Items.Count == 0)
        {
            //플레이어를 처음 생성하면서 아이템 공간이 0일 때
            //25칸의 빈 아이템 칸을 지급해줌

            for (var i = 0; i < 25; i++)
            {
                var bItem = ItemManager.Instance.CodeToItem(0);
                items.Add(bItem);
            }
        }
        //인스펙터에서 추가한 아이템을 적용
        for (var i = 0; i < itemCodeAmount.Count; i++)
        {
            var item = ItemManager.Instance.CodeToItem(itemCodeAmount[i].itemCode);
            item.ItemAmount = itemCodeAmount[i].itemAmount;
            items.Add(item);
        }
        if (isEnemy == false)
        {
            weapon = new Item(0);
            accessory = new Item(0);
            armor = new Item(0);
        }
    }
    #endregion
}

