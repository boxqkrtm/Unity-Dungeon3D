using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 모든 몬스터의 공통 스크립트, 데미지를 입거나 사망처리 관리
/// </summary>
public class Monster : MonoBehaviour
{
    public UnitData ud;
    [HideInInspector]
    public Animator animator;
    Collider myCol;
    public Image hpBar;
    public TextMeshProUGUI monsterNameText;
    GameObject player;
    Rigidbody rb;

    public OnDead onDead;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        var monCanvas = transform.Find("SimpleMobInfoCanvas");
        hpBar = monCanvas.Find("HP").GetComponent<Image>();
        monsterNameText = monCanvas.Find("NAME").GetComponent<TextMeshProUGUI>();
        monsterNameText.text = ud.Name + " Lv" + ud.Lv.ToString();
        player = GameManager.Instance.PlayerObj;
        myCol = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        ud.UnitDataInit();
        ud.onHpChanged += OnHpChanged;
        StartCoroutine(BuffLoop());
    }

    IEnumerator BuffLoop()
    {
        while (true)
        {
            yield return null;
            ud.buffLoop(gameObject);
            monsterNameText.text = ud.Name + " Lv" + ud.Lv.ToString();
        }
    }

    public void OnHpChanged()
    {
        hpBar.fillAmount = ud.HpRatio;
        if (ud.Hp == 0)
        {
            StartCoroutine(MobDespawn());
        }
    }

    IEnumerator MobDespawn()
    {
        onDead();//delegate
        myCol.enabled = false;
        rb.isKinematic = true;
        animator.SetBool("IsDead", true);
        GameManager.Instance.PlayerGetExpByKill(ud.Exp);
        GameManager.Instance.PlayerGetGoldByKill(ud.Gold);
        yield return new WaitForSeconds(1.6f);
        EffectManager.Instance.CreateMobDespawnEffect(transform.position);
        if (ud.Items.Count > 0)
        {
            var rand = Random.Range(0, ud.Items.Count);
            if (ud.Items[rand].ItemCode != 0)
            {
                EffectManager.Instance.CreateDroppedItem(transform.position, ud.Items[rand]);
            }
        }
        Destroy(gameObject);
    }

    public delegate void OnDead();

    public void TakeDamage(AttackType at, int amount)
    {
        if (ud.Hp == 0) return;
        var damage = ud.TakeDamageCalc(at, amount);

        //랜덤계산으로 데미지가 중간 값 기준 몇배 들어갔는지
        var effective = ud.CalculateDamageEffective(damage, amount);

        animator.SetTrigger("TriggerTakeDamage");
        Vector3 dir = (transform.position - player.transform.position).normalized;
        //랜덤 계산에서 데미지가 더 많이 들어갔을 수록 데미지를 더 크게 표시함
        EffectManager.Instance.CreateHitDamageText(transform.position, damage, effective >= 1.15 ? effective * 2 : effective);
        GetComponent<Rigidbody>().AddForce(dir * 10000f);
        ud.Hp -= damage;
    }
}