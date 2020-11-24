using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//기본 불속성의 슬라임
public class GolemAI : MonsterAI
{
    public GameObject missale;
    public GameObject warningAlert;
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
    bool skillPattern = false;
    float timer = 0f;
    public override void Idle()
    {
        monster.animator.SetTrigger("TriggerIdle");
        state = MonsterAIState.Chase;
        timer += Time.deltaTime;
        if (timer >= 10f) { state = MonsterAIState.BossSkill; timer = 0f; }
        if (timer >= 7f) { warningAlert.SetActive(true); } else warningAlert.SetActive(false);
    }

    public override void Chase()
    {
        monster.animator.SetTrigger("TriggerMove");
        Vector3 target = PlayerPos - MyPos;
        target = target.normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target), 180 * Time.deltaTime);
        //rigidbody.velocity = new Vector3(target.x, 0f, target.z) * monster.ud.Speed;
        Agent.destination = PlayerPos;
        if (PlayerDistance < 3f)
            state = MonsterAIState.Attack;
        timer += Time.deltaTime;
        if (timer >= 10f) { state = MonsterAIState.BossSkill; timer = 0f; }
        if (timer >= 7f) { warningAlert.SetActive(true); } else warningAlert.SetActive(false);
    }

    public override void Attack()
    {
        Vector3 target = PlayerPos - MyPos;
        target = target.normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target), 180 * Time.deltaTime);
        monster.animator.SetTrigger("TriggerAttack");
        if (PlayerDistance > 3f)
            state = MonsterAIState.Chase;
        timer += Time.deltaTime;
        if (timer >= 10f) { state = MonsterAIState.BossSkill; timer = 0f; }
        if (timer >= 7f) { warningAlert.SetActive(true); } else warningAlert.SetActive(false);
    }

    public void AnimAttackSucceed()
    {
        if (PlayerDistance <= 3f && monster.ud.Hp > 0)
        {
            player.GetComponent<Player>().TakeDamage(AttackType.Fire, monster.ud.Atk);
            EffectManager.Instance.CreateHitSwordEffect(PlayerPos);
        }
    }

    public override void BossSkill()
    {
        timer += Time.deltaTime;
        monster.animator.SetTrigger("TriggerIdle");
        if (timer >= 10f)
        {
            state = MonsterAIState.Idle;
            timer = 0;
            if (BossSkillRoutine != null)
            {

                StopCoroutine(BossSkillRoutine);
                BossSkillRoutine = null;
            }
            if (BossAroundDamageRoutine != null)
            {
                StopCoroutine(BossAroundDamageRoutine);
                BossAroundDamageRoutine = null;
            }
        }
        else
        {
            if (BossSkillRoutine == null)
                BossSkillRoutine = StartCoroutine(BossSkillMissile());
            if (BossAroundDamageRoutine == null)
                BossAroundDamageRoutine = StartCoroutine(BossAroundDamage());
        }
    }

    Coroutine BossSkillRoutine = null;
    Coroutine BossAroundDamageRoutine = null;
    IEnumerator BossSkillMissile()
    {
        while(true)
        {

        yield return new WaitForSeconds(0.5f);
        {
            var obj = Instantiate(missale, transform.position, Quaternion.identity);
            var objs = obj.GetComponent<MagicMissale>();
            switch (Random.Range(0, 4))
            {
                case 0:
                    objs.attackType = AttackType.Fire;
                    break;
                case 1:
                    objs.attackType = AttackType.Ice;
                    break;
                case 2:
                    objs.attackType = AttackType.Light;
                    break;
                case 3:
                    objs.attackType = AttackType.Poison;
                    break;
            }
            objs.attackDamage = monster.ud.Atk;
            objs.target = GameManager.Instance.PlayerObj.transform.position;
        }
        }
    }
    IEnumerator BossAroundDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (PlayerDistance <= 5f && monster.ud.Hp > 0)
                GameManager.Instance.PlayerScript.TakeDamage(AttackType.None, monster.ud.Atk / 100);
        }
    }

    public override void RandomMove()
    {

    }

    public override void OnDead()
    {
        StartCoroutine(End());
    }
    IEnumerator End()
    {
        EffectManager.Instance.StopEffect(5f);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.MoveScene("End",Vector3.zero);
    }
}
