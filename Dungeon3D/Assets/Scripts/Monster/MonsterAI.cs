using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.
public enum MonsterAIState
{
    Idle, Chase, Attack, RandomMove, BossSkill
}
public class MonsterAI : MonoBehaviour
{
    [HideInInspector]
    public Monster monster;//스테이터스 관리용
    [HideInInspector]
    public Rigidbody rigidbody;
    public MonsterAIState state = MonsterAIState.Idle;
    [HideInInspector]
    public GameObject player;
    public float PlayerDistance
    {
        get { return Vector3.Distance(player.transform.position, transform.position); }
    }
    public Vector3 PlayerPos
    {
        get
        {
            return player.transform.position;
        }
    }
    public Vector3 MyPos
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        player = GameManager.Instance.PlayerObj;
        monster = gameObject.GetComponent<Monster>();
        monster.onDead = new Monster.OnDead(OnDead);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (monster.ud.Hp != 0) Think();
        else
        {
            var raw = Vector3.zero;
            rigidbody.velocity = new Vector3(raw.x, rigidbody.velocity.y, raw.z);
        }
    }

    void Think()
    {
        if(PlayerDistance > 10f)
        {
            for(var i =0;i<3;i++)
                transform.Find("SimpleMobInfoCanvas").GetChild(i).gameObject.SetActive(false);
        }
        else
        {
            for(var i =0;i<3;i++)
                transform.Find("SimpleMobInfoCanvas").GetChild(i).gameObject.SetActive(true);

        }
        if (state == MonsterAIState.Idle)
        {
            Idle();
            var raw = Vector3.zero;
            rigidbody.velocity = new Vector3(raw.x, rigidbody.velocity.y, raw.z);
        }
        else if (state == MonsterAIState.Chase) Chase();
        else if (state == MonsterAIState.Attack) Attack();
        else if (state == MonsterAIState.RandomMove) RandomMove();
        else if (state == MonsterAIState.BossSkill) BossSkill();
    }

    virtual public void OnDead() { }
    virtual public void Idle() { }
    virtual public void Chase() { }
    virtual public void Attack() { }
    virtual public void RandomMove() { }
    virtual public void BossSkill() { }
}
