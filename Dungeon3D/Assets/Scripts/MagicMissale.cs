using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissale : MonoBehaviour
{
    public AttackType attackType;
    public int attackDamage;
    public Vector3 target;
    Material m;
    void Start()
    {
        m = GetComponent<MeshRenderer>().material;
        StartCoroutine(AutoDestroy());
    }
    IEnumerator AutoDestroy()
    {
        yield return null;
        while (m.color.a > 0)
        {
            Color c = m.color;
            c.a -= Time.deltaTime / 2;
            m.color = c;
        }
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 5f);
        transform.LookAt(target);
        Debug.Log(Vector3.Distance(GameManager.Instance.PlayerObj.transform.position, transform.position));
        if (Vector3.Distance(GameManager.Instance.PlayerObj.transform.position, transform.position) <= 1f)
        {
            GameManager.Instance.PlayerScript.TakeDamage(attackType, attackDamage);
            Buff buff = null;
            switch (attackType)
            {
                case AttackType.Fire:
                    buff = new Buff("화상", BuffType.Hp, AttackType.Fire, BuffIcon.FireIcon, attackDamage / 5, 3.0f);
                    break;
                case AttackType.Ice:
                    buff = new Buff("냉기", BuffType.Speed, AttackType.Ice, BuffIcon.FireIcon, attackDamage / 5, 3.0f);
                    break;
                case AttackType.Light:
                    buff = new Buff("감전", BuffType.Speed, AttackType.Light, BuffIcon.FireIcon, attackDamage / 5, 3.0f);
                    break;
                case AttackType.Poison:
                    buff = new Buff("독", BuffType.Hp, AttackType.Poison, BuffIcon.FireIcon, attackDamage / 5, 10.0f);
                    break;
            }
            GameManager.Instance.PlayerScript.TakeSpecial(buff, 0.5f);
            //타격
        }
    }
}
