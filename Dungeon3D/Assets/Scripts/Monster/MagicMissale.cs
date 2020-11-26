using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissale : MonoBehaviour
{
    public AttackType attackType;
    public int attackDamage;
    public Vector3 target;
    Material m;
    public Color color;
    void Start()
    {
        m = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        StartCoroutine(AutoDestroy());
        StartCoroutine(Shot());
    }
    IEnumerator AutoDestroy()
    {
        while (m.color.a > 0)
        {
            yield return null;
            Color c = color;
            color.a -= Time.deltaTime / 5;
            m.color = color;
        }
        Destroy(gameObject);
    }
    IEnumerator Shot()
    {
        //위로 솟음
        var randx = Random.Range(-1f, 1f);
        var randz = Random.Range(-1f, 1f);
        while(transform.position.y < 4f)
        {
            yield return null;
            var pos = transform.position;
            pos.y += 1f * Time.deltaTime * 12f;
            pos.x += 1f * Time.deltaTime * randx;
            pos.z += 1f * Time.deltaTime * randz;
            transform.position = pos;
        }
        SEManager.Instance.Play(SEManager.Instance.shotSE);
        //내려꽂음
        while(true)
        {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 20f);
            transform.LookAt(target);
            if (Vector3.Distance(GameManager.Instance.PlayerObj.transform.position, transform.position) <= 1f)
            {
                Buff buff = null;
                switch (attackType)
                {
                    case AttackType.Fire:
                        buff = new Buff("화상", BuffType.Hp, AttackType.Fire, BuffIcon.FireIcon, -attackDamage / 5, 3.0f);
                        SEManager.Instance.Play(SEManager.Instance.fireSE);
                        break;
                    case AttackType.Ice:
                        buff = new Buff("냉기", BuffType.Speed, AttackType.Ice, BuffIcon.IceIcon, 0.8f, 3.0f);
                        SEManager.Instance.Play(SEManager.Instance.iceSE);
                        break;
                    case AttackType.Light:
                        buff = new Buff("감전", BuffType.Speed, AttackType.Light, BuffIcon.LightningIcon, 0.8f, 3.0f);
                        SEManager.Instance.Play(SEManager.Instance.lightningSE);
                        break;
                    case AttackType.Poison:
                        buff = new Buff("독", BuffType.Hp, AttackType.Poison, BuffIcon.PoisonIcon, -attackDamage / 5, 10.0f);
                        SEManager.Instance.Play(SEManager.Instance.poisonSE);
                        break;
                }
                GameManager.Instance.PlayerScript.TakeDamage(attackType, attackDamage);
                GameManager.Instance.PlayerScript.TakeSpecial(buff, 1f);
                //타격
                Destroy(gameObject);
            }
        }
    }
}
