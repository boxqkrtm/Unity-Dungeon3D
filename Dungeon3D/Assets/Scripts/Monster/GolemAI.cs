﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

//기본 불속성의 슬라임
public class GolemAI : MonsterAI
{
    public GameObject missale;
    public GameObject warningBossSkillAlert;
    public GameObject warningSpaceAlert;
    public GameObject beam;
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
    float timer = 0f;
    public override void Idle()
    {
        monster.animator.SetTrigger("TriggerIdle");
        state = MonsterAIState.Chase;
        timer += Time.deltaTime;
        if (timer >= 10f) { state = MonsterAIState.BossSkill; timer = 0f; }
        if (timer >= 7f) { warningBossSkillAlert.SetActive(true); } else warningBossSkillAlert.SetActive(false);
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
        if (timer >= 7f) { warningBossSkillAlert.SetActive(true); } else warningBossSkillAlert.SetActive(false);
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
        if (timer >= 7f) { warningBossSkillAlert.SetActive(true); } else warningBossSkillAlert.SetActive(false);
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
        if (timer >= 7f)
        {
            state = MonsterAIState.BossSkill2;
            timer = 0;
            if (BossSkillRoutine != null)
            {

                StopCoroutine(BossSkillRoutine);
                BossSkillRoutine = null;
            }
        }
        else
        {
            if (BossSkillRoutine == null)
                BossSkillRoutine = StartCoroutine(BossSkillMissile());
        }
    }
    public override void BossSkill2()
    {
        timer += Time.deltaTime;
        monster.animator.SetTrigger("TriggerIdle");
        if (timer >= 2.1f)
        {
            state = MonsterAIState.Idle;
            timer = 0;
            beam.SetActive(false);
            if (BossSkillRoutine != null)
            {
                StopCoroutine(BossSkillRoutine);
                BossSkillRoutine = null;
            }
        }
        else
        {
            if (BossSkillRoutine == null)
                BossSkillRoutine = StartCoroutine(BossSkillBeam());
        }
    }
    IEnumerator BossSkillBeam()
    {
        Vector3 tempTarget;
        beam.SetActive(true);
        var zeroPos = transform.position;
        zeroPos.y = 2f;
        Vector3 target = zeroPos;
        float deltaTime = 0;
        float ddeltaTime = 0;
        beam.GetComponent<Lazer>().start = zeroPos;
        SEManager.Instance.Play(SEManager.Instance.lazerSE);    
        var playerPos = GameManager.Instance.PlayerObj.transform.position;
        playerPos.y = 0;
        target.y = 0;
        target = Vector3.Lerp(target, playerPos, 0.8f);
        tempTarget = target;
        tempTarget.y = 1f;
        beam.GetComponent<Lazer>().target = tempTarget;

        //yield return new WaitForSeconds(0.3f);
        while (2f >= deltaTime)
        {
            yield return null;
            ddeltaTime += Time.deltaTime;
            deltaTime += Time.deltaTime;
            playerPos = GameManager.Instance.PlayerObj.transform.position;
            playerPos.y = 0;
            target.y = 0;
            target = Vector3.MoveTowards(target, playerPos, Time.deltaTime * 7.5f);
            if (Vector3.Distance(target, playerPos) <= 0.5f)
            {
                GameManager.Instance.PlayerScript.TakeDamage(AttackType.Light, Mathf.RoundToInt(monster.ud.Atk * 1.5f));
                yield return new WaitForSeconds(0.5f);
            }
            if(ddeltaTime >= 0.1f)
            {
                EffectManager.Instance.CreateLazerGroundEffect(target);
                ddeltaTime = 0f;
            }
            
            tempTarget = target;
            tempTarget.y = 1f;
            beam.GetComponent<Lazer>().target = tempTarget;
        }
        beam.SetActive(false);

    }

    Coroutine BossSkillRoutine = null;
    IEnumerator BossSkillMissile()
    {
        for (var j = 0; j < 4; j++)
        {
            var delta =  GameManager.Instance.PlayerObj.transform.rotation * Vector3.forward * 2;
            var playerPos = GameManager.Instance.PlayerObj.transform.position;
            for (var i = 0; i < 4; i++)
                Instantiate(warningSpaceAlert, playerPos + delta * i, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            for (var i = 0; i < 4; i++)
            {
                var obj = Instantiate(missale, transform.position, Quaternion.identity);
                var objs = obj.GetComponent<MagicMissale>();
                switch (j)
                {
                    case 0:
                        objs.attackType = AttackType.Fire;
                        objs.color = Color.red;
                        break;
                    case 1:
                        objs.attackType = AttackType.Ice;
                        objs.color = Color.blue;
                        break;
                    case 2:
                        objs.attackType = AttackType.Light;
                        objs.color = Color.yellow;
                        break;
                    case 3:
                        objs.attackType = AttackType.Poison;
                        objs.color = Color.green;
                        break;
                }
                objs.attackDamage = monster.ud.Atk;
                objs.target = playerPos + delta * i;
                Instantiate(warningSpaceAlert, objs.target, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.7f);
        }
        while (true)
        {
            yield return null;
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

        var ps = InputManager.Ps;
        var location = new Vector3(21.76715f, -0.02960289f, -39.73073f);
        ps.Disable();
        GameManager.Instance.StopAutoSave();
        GameManager.Instance.PlayerScript.ud.Location = new Vector3Data(location);
        GameManager.Instance.PlayerScript.ud.SceneName = "mainTown";
        GameManager.Instance.SaveGame();
        SceneManager.LoadScene("End");
    }
}
