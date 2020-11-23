using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기본 불속성의 슬라임
public class SlimeAI : MonsterAI
{
    public float sight = 5f;
    public override void Idle()
    {
        monster.animator.SetTrigger("TriggerIdle");
        if (PlayerDistance <= sight)
            state = MonsterAIState.Chase;
    }

    public override void Chase()
    {
        monster.animator.SetTrigger("TriggerMove");
        Vector3 target = PlayerPos - MyPos;
        target = target.normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target), 180 * Time.deltaTime);
        var raw = new Vector3(target.x, 0, target.z) * monster.ud.Speed;
        rigidbody.velocity = new Vector3(raw.x, rigidbody.velocity.y, raw.z);
        if (PlayerDistance > sight)
            state = MonsterAIState.Idle;
        if (PlayerDistance < 2f)
            state = MonsterAIState.Attack;
    }

    public override void Attack()
    {
        Vector3 target = PlayerPos - MyPos;
        target = target.normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target), 180 * Time.deltaTime);
        monster.animator.SetTrigger("TriggerAttack");
        if (PlayerDistance > 1.5f)
            state = MonsterAIState.Chase;
    }

    public void AnimAttackSucceed()
    {
        if (PlayerDistance <= 1.5f && monster.ud.Hp > 0)
        {
            player.GetComponent<Player>().TakeDamage(AttackType.Fire, monster.ud.Atk);
            var buff = new Buff();
            buff.BuffType = BuffType.Hp;
            buff.BuffResistType = AttackType.Fire;
            buff.SetBuffIcon(BuffIcon.FireIcon);
            buff.BuffPower = -1f * monster.ud.Atk;
            buff.BuffDuration = 3f;
            buff.BuffName = "화상";
            player.GetComponent<Player>().TakeSpecial(buff, 0.3f);
            EffectManager.Instance.CreateHitFireEffect(PlayerPos);
        }
    }

    public override void RandomMove()
    {

    }
}
