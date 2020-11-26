using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillFunction
{
    Skill1
}
[System.Serializable]
public class Skill
{
    #region Init
    public Skill(string skillName, int skillLevel, int maxSkillLevel, SkillFunction skillFunction
    , float skillDelay, int mp, int hp = 0, int gold = 0)
    {
        this.skillName = skillName;
        this.skillLevel = skillLevel;
        this.maxSkillLevel = maxSkillLevel;
        this.skillFunction = skillFunction;
        this.skillDelay = skillDelay;
        this.skillDelayTimer = 0;
        this.mp = mp;
        this.hp = hp;
        this.gold = gold;
    }
    #endregion
    #region Skill Info
    string skillName;
    public string SkillName { get => skillName; }
    int skillLevel;
    public int SkillLv { get => skillLevel; set => skillLevel = value; }
    int maxSkillLevel; //스킬포인트로 강화가능 1부터 스킬 있음
    public int MaxSkillLevel { get { return maxSkillLevel; } }
    SkillFunction skillFunction; // 스킬이 실행 할 함수
    float skillDelayTimer;
    public float SkillDelayTimer { get => skillDelayTimer; set => skillDelayTimer = value; }
    float skillDelay;
    #endregion
    #region Function
    public void SkillDelayLoop() //스킬 쿨다운 계산을 위한 루프 함수
    {
        Timer.Update(ref skillDelayTimer);
    }
    public void ExecuteSkill()
    {
        if (skillDelayTimer <= 0f)
        {
            skillDelayTimer = skillDelay;
            var ps = GameManager.Instance.PlayerScript;
            ps.ud.Mp -= Mp;
            switch (skillFunction)
            {
                case SkillFunction.Skill1:
                    Buff buff = new Buff("파이널 슬래시", BuffType.Atk, AttackType.None, BuffIcon.RespawnBuffIcon, ps.ud.PureAtk * 1.1f, 3f, "스킬");
                    ps.ud.Buffs.Add(buff);
                    ps.Animator.SetTrigger("TriggerSkill1");
                    break;
            }
        }
        else
        {
            AlertManager.Instance.AddGameLog($"쿨타임이 {skillDelayTimer.ToString("F1")}초 남았습니다.");
        }
    }
    #endregion
    #region Resource For Skill
    int hp;
    public int Hp { get { return hp; } set { hp = value; } }
    int mp;
    public int Mp { get { return mp; } set { mp = value; } }
    int gold;
    public int Gold { get { return gold; } set { gold = value; } }
    #endregion
}
