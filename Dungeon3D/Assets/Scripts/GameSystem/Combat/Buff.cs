using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public enum BuffType //버프가 건들이는 능력치
{
    Hp, Mp, Speed, Atk, Def
    //hp mp는 도트 데미지
    //speed atk def는 버프가 끝날 때 까지 효과지속
    //같은 디버프 or 버프의 경우 강도가 강한 버프 우선 적용
    //더 약한 강도의 버프는 더 강한 버프가 끝날 때 까지 무시( 버프 시간은 닳음 )
}

[System.Serializable]
public class Buff
{
    BuffType buffType;
    public BuffType BuffType { get; set; }
    //어떤 종류로 저항할 수 있는지 //ex 불의 경우 화상치료제 등으로 제거, 불 내성에 따른 buffpower 저하
    public AttackType BuffResistType { get; set; }
    BuffIcon buffIcon;
    public Sprite _BuffIcon { get { return BuffManager.Instance.GetBuffIcon(buffIcon); } }
    public void SetBuffIcon(BuffIcon value){ buffIcon = value; } 
    public float BuffPower { get; set; } //버프 한개의 강도 (음수로 디버프 가능)
    float buffDuration;
    public float BuffDuration { get { return buffDuration; } set { buffDuration = value; maxBuffDuration = value; } }
    public void SubBuffDuration(float value) { buffDuration -= value; }
    float maxBuffDuration;
    public float BuffDurationRatio { get { return BuffDuration / maxBuffDuration; } }
    public float BuffDelta { get; set; } //버프 도트 데미지용 1까지 쌓았다가 1이 쌓이면 1회 데미지가 가는식
    string buffName = "";
    public string BuffName
    {
        get
        {
            return buffName;
        }
        set
        {
            buffName = value;
        }
    }
    string buffInfo = "";
    public string BuffInfo
    {
        get
        {
            string result = "";
            result += BuffName + "\n";
            result += buffInfo;
            if (buffInfo != "") result += "\n";
            var x = BuffPower.ToString();
            var mx = (-BuffPower).ToString();
            if (BuffPower > 0)
            {
                switch (BuffType)
                {
                    case BuffType.Hp:
                        result += "1초마다 체력을 " + x + "만큼 회복한다.";
                        break;
                    case BuffType.Mp:
                        result += "1초마다 마나를 " + x + "만큼 회복한다.";
                        break;
                    case BuffType.Speed:
                        result += "기본 이동속도를 " + x + "배로 변경한다.";
                        break;
                    case BuffType.Atk:
                        result += "공격력을 " + x + "만큼 증가시킨다.";
                        break;
                    case BuffType.Def:
                        result += "방어력을 " + x + "만큼 증가시킨다.";
                        break;
                }
            }
            else
            {
                switch (BuffType)
                {
                    case BuffType.Hp:
                        result += "1초마다 체력을 " + mx + "만큼 잃는다.";
                        break;
                    case BuffType.Mp:
                        result += "1초마다 마나를 " + mx + "만큼 잃는다.";
                        break;
                    case BuffType.Atk:
                        result += "공격력을 " + mx + "만큼 감소시킨다.";
                        break;
                    case BuffType.Def:
                        result += "방어력을 " + mx + "만큼 감소시킨다.";
                        break;
                }
            }
            return result;
        }
        set { buffInfo = value; }
    }
    public Buff()
    {

    }
    public Buff(string buffName="None", BuffType buffType=BuffType.Hp, 
        AttackType buffResistType=AttackType.None, 
        BuffIcon buffIcon=BuffIcon.FireIcon, float buffPower =1f, 
			float buffDuration = 1f, string buffInfo = "" )
	{
        BuffName = buffName;
        BuffType = buffType;
        BuffResistType = buffResistType;
        SetBuffIcon(buffIcon);
        BuffPower = buffPower;
        BuffDuration = buffDuration;
        BuffInfo = buffInfo;
		
	}
}
