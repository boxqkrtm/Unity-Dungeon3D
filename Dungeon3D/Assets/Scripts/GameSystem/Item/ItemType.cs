using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Sword, Staff, Wand, Bow, Arrow,
    FireStatePotion, IceStatePotion, PoisonStatePotion,
    HealPotion, Armor, Accessories, Special, Null, HealMpPotion, SpeedBuffPotion
}

//Fire Ice Poison 포션은 파워를 양수로 둬서
//걸린 상태이상의 파워보다 포션이 더 높으면 상태이상을 해제한다.포션이 더 높으면 상태이상을 해제한다. 상태이상을 해제한다.포션이 더 높으면 상태이상을 해제한다.션이 더 높으면 상태이상을 해제한다. 상태이상을 해제한다.포션이 더 높으면 상태이상을 해제한다.
//Special로 시작하는 아이템은 퀘스트와 같은 전용아이템, 판매, 버리기 불가능
