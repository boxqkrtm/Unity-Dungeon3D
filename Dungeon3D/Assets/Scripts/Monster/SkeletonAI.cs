using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//기본 불속성의 슬라임
public class SkeletonAI : MonsterAI
{
    NavMeshAgent agent;
    NavMeshAgent Agent
    {
        get
        {
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }
            return agent;
        }
    }
    public override void Idle()
    {
        monster.animator.SetTrigger("TriggerIdle");
        if (PlayerDistance <= 14f)
            state = MonsterAIState.Chase;
    }

    public override void Chase()
    {
        monster.animator.SetTrigger("TriggerMove");
        Vector3 target = PlayerPos - MyPos;
        target = target.normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target), 180 * Time.deltaTime);
        //rigidbody.velocity = new Vector3(target.x, 0f, target.z) * monster.ud.Speed;
        Agent.destination = PlayerPos;
        if (PlayerDistance > 14f)
            state = MonsterAIState.Idle;
        if (PlayerDistance < 3f)
            state = MonsterAIState.Attack;
    }

    public override void Attack()
    {
        Vector3 target = PlayerPos - MyPos;
        target = target.normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target), 180 * Time.deltaTime);
        monster.animator.SetTrigger("TriggerAttack");
        if (PlayerDistance > 3f)
            state = MonsterAIState.Chase;
    }

    public void AnimAttackSucceed()
    {
        if (PlayerDistance <= 3f && monster.ud.Hp > 0)
        {
            player.GetComponent<Player>().TakeDamage(AttackType.None, monster.ud.Atk);
            EffectManager.Instance.CreateHitSwordEffect(PlayerPos);
        }
    }

    public override void RandomMove()
    {

    }

    public override void OnDead()
    {
        foreach (var i in QuestManager.Instance.PlayerQuests)
        {
            if (i.QuestType == QuestType.KillSkeleton)
            {
                i.ProgressValue += 1;
            }
        }
    }
}
