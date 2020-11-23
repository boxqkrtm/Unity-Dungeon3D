using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine.Utility;

public class Player : MonoBehaviour
{
    #region Variables
    public GameObject buffZone;
    public Image hpBar, mpBar, expBar;
    public Text hpText, mpText, expText;
    public TextMeshProUGUI lvText;

    float sprintMultiplyer = 1.5f;
    float isAttackAble = 0; //0이상시 공격 판정 가능상태
    float takeDamageAnimCoolDown = 0.5f;
    float takeDamegeAnimCoolDownTimer = 0f;
    float sprintBoosting = 0f;
    float sprintBoostingCooldownTimer = 0f;
    float sprintBoostingCooldown = 0.50f;

    Rigidbody rb;
    Collider col;
    Animator animator;
    public Animator Animator { get => animator; }
    public UnitData ud;
    public Collider mySwordHitbox;
    public GameObject mySwordTrail;
    SMRAfterImageCreator aic;

    //bool for state and anim
    bool isAttack = false;
    bool isSprint = false;
    bool isMove = false;

    public int Lv { get { return ud.Lv; } }
    #endregion
    void Start()
    {
        //AddAfterImage
        aic = GetComponent<SMRAfterImageCreator>();
        aic.Setup(transform.GetComponentInChildren<SkinnedMeshRenderer>(), 100, 1f);
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        StartCoroutine(DelayLoop());
    }
    public void InitUd()
    {
        ud.onHpChanged += OnHpChanged;
        ud.onMpChanged += OnMpChanged;
        ud.onExpChanged += OnExpChanged;
        ud.onLvChanged += OnLvChanged;
        OnHpChanged();
        OnMpChanged();
        OnExpChanged();
        //ud.Lv = ud.Lv;
        lvText.text = ud.Lv.ToString();
    }
    IEnumerator DelayLoop()
    {
        while (true)
        {
            yield return null;
            ud.buffLoop(this.gameObject, buffZone);
            ud.SkillLoop();
        }
    }
    int befHp;
    bool isDead;
    List<GameObject> dashSkillTargets = new List<GameObject>();
    bool isPlayerCameraLocked = false;
    void Update()
    {
        if (transform.position.y < -100f) Dead();
        if (ud == null) return; //ud 받아올 때 까지 대기
        if (ud.Hp == 0 && isDead == false) { Dead(); }
        RunCoolDown();
        //사망 시 또는 피격 모션 중 경직
        if (ud.Hp == 0 || animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit_SwordShield"))
        { rb.velocity = Vector3.zero; isAttackAble = 0f; return; }

        //공격 모션 중 이동 불가능
        if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack01_SwordShield") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack02_SwordShield")))
        {
            MoveWithDirection();
        }

        //공격이 끝난 순간 약간의 방향 전환 가능
        if (isAttackAble <= 0) MoveWithDirection(true);


        //애니메이션 설정
        animator.SetBool("IsAttack", isAttack);
        if (isAttack == true)
        {
            var raw = Vector3.zero;
            raw = new Vector3(raw.x, rb.velocity.y, raw.z);
            rb.velocity = raw;
        }
        //대쉬어택의 경우 공격중 속도 0 무시 관통효과
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DashAttack") && ud.Mp >= 1)
        {
            ud.MpFloatMinus += 5 * Time.deltaTime;
            gameObject.layer = 10;//몹 임시 관통
            var raw = transform.rotation * Vector3.forward * ud.Speed * 8;
            rb.velocity = raw;
        }
        else
            gameObject.layer = 8;//몹 관통 해제
        //대쉬스킬어택의 경우 공격중 속도 0 무시 관통효과
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DashSkillAttack") && ud.Mp >= 10 && isAttackAble > 0)
        {
            if (dashSkillTargets.Count >= 3)
            {
                animator.SetTrigger("TriggerSkill1End");
                rb.velocity = Vector3.zero;
                AttackEndEvent();
                return;
            }
            gameObject.layer = 10;//몹 임시 관통
            var minTargetDisTance = 15f;
            GameObject DashSkillminTarget = null;
            foreach (var mon in GameObject.FindGameObjectsWithTag("Monster"))
            {
                //부적합 타겟
                var isHitted = false;
                foreach (var monmon in dashSkillTargets)
                    if (mon != null && monmon != null)
                        if (monmon.gameObject.GetInstanceID() == mon.gameObject.GetInstanceID())
                            isHitted = true;
                if (isHitted == true) continue;
                if (mon?.GetComponent<Monster>()?.ud.Hp == 0) continue;
                if (Mathf.Abs(mon.transform.position.y - transform.position.y) >= 1f) continue;
                if (Vector3.Distance(mon.transform.position, transform.position) > minTargetDisTance) continue;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 15f))
                {
                    Debug.Log("hit");
                }
                else
                {
                    Debug.Log("no hit");
                    continue;
                }

                //추적 할 타겟 설정
                DashSkillminTarget = mon;
                minTargetDisTance = Vector3.Distance(mon.transform.position, transform.position);
            }
            if (DashSkillminTarget != null)
            {
                var vecDir = DashSkillminTarget.transform.position - transform.position;
                var rotation = -Mathf.Atan2(vecDir.z, vecDir.x) * Mathf.Rad2Deg;
                var rotQuaternion = Quaternion.Euler(0, rotation + 90f, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotQuaternion, 360f * Time.deltaTime);
                EffectManager.Instance.UnLockCameraTargetEffect();
                EffectManager.Instance.LockCameraTargetEffect(DashSkillminTarget.transform, new Vector3(0f, 11.12f, -20.29f), 0);
                isPlayerCameraLocked = true;
                var raw = transform.rotation * Vector3.forward * ud.Speed * 24;
                rb.velocity = raw;
                if (Vector3.Distance(DashSkillminTarget.transform.position, transform.position) < 0.5f)
                {
                    dashSkillTargets.Add(DashSkillminTarget);
                    GameManager.Instance.PlayerSwordHit(DashSkillminTarget, DashSkillminTarget.transform.position);
                }
            }
        }
        else
        {
            if (isPlayerCameraLocked == true)
            {
                EffectManager.Instance.UnLockCameraTargetEffect();
                isPlayerCameraLocked = false;
                EffectManager.Instance.StopEffect(0.7f);
            }
            dashSkillTargets.Clear();
            gameObject.layer = 8;//몹 관통 해제
        }

        //속도가 충분히 빠른 경우 잔상 생성
        if (Vector3.Distance(Vector3.zero, rb.velocity) > 10f)
        {
            aic.Create(true);
            var raw = rb.velocity;
            raw.y = 0;
            rb.velocity = raw;
        }

        else aic.Create(false);
        animator.SetBool("IsMove", isMove);
        animator.SetBool("IsSprint", isSprint);
        animator.SetFloat("moveSpeed", Vector3.Distance(Vector3.zero, rb.velocity));
    }

    public void Skill1()
    {
        ud.Skills[0]?.ExecuteSkill();
    }
    public void Skill2()
    {
        ud.Skills[1]?.ExecuteSkill();
    }
    public void Skill3()
    {
        ud.Skills[2]?.ExecuteSkill();
    }
    public void Respawn()
    {
        EffectManager.Instance.CreateLevelupEffect(transform.position);
        isDead = false;
        col.enabled = true;
        rb.isKinematic = false;
        animator.SetBool("IsDead", false);
    }
    //InputManager에서 실행
    public void Attack()
    {
        isAttack = true;
    }
    public void AttackOff()
    {
        isAttack = false;
    }
    public void DashBoost()
    {
        if (sprintBoostingCooldownTimer <= 0f)
        {
            if (ud.Mp >= 2)
            {
                ud.Mp -= 2;
                EffectManager.Instance.CreateDashBoostEffect(transform.position);
                sprintBoosting = 2f;
                sprintBoostingCooldownTimer = sprintBoostingCooldown;
            }
        }
    }

    bool isDashPressed = false;
    public void Dash()
    {
        isSprint = true;
    }
    public void DashOff()
    {
        isSprint = false;
    }

    //검 공격 연산
    public void AttackEvent()//공격 애니메이션 중 피격 판정이 일어나야 할 시점에 호출됨
    {
        mySwordHitbox.enabled = false;
        mySwordHitbox.enabled = true;
        mySwordTrail.GetComponent<TrailRenderer>().emitting = true;
        isAttackAble = 1f;
    }
    public void AttackEndEvent()
    {
        mySwordHitbox.enabled = false;
        mySwordTrail.GetComponent<TrailRenderer>().emitting = false;
        isAttackAble = 0f;
    }
    public void PlayerSwordHit(GameObject go, Vector3 hitPosition)
    {
        if (go.CompareTag("MapArea")) return;
        if (isAttackAble > 0f)
        {
            EffectManager.Instance.CreateHitSwordEffect(hitPosition);
            //effect
            if (go.CompareTag("Monster"))
            {

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("DashSkillAttack"))
                {
                    dashSkillTargets.Add(go);
                }
                //Debug.Log("몬스터 공격");
                Monster mon = go.GetComponent<Monster>();
                //들고있던 검의 히트이므로 무조건 Atk데미지
                mon.TakeDamage(AttackType.Atk, ud.Atk);
            }
            //damage
        }
    }

    //능력치 변동
    public void GetExp(int amount)
    {
        ud.Exp += amount;
        //Debug.Log("지금 경험치 : " + ud.Exp.ToString());
    }
    public void TakeDamage(AttackType at, int amount)
    {
        if (ud.Hp == 0) return;
        ud.Hp -= ud.TakeDamageCalc(at, amount);
    }

    public void TakeSpecial(Buff buff, float chance)
    {
        if (ud.Hp == 0) return;
        ud.TakeSpecial(buff, chance);
    }

    //프레임마다 줄어들어야 할 변수들을 줄여주는 함수
    void RunCoolDown()
    {
        if (takeDamegeAnimCoolDownTimer > 0f) takeDamegeAnimCoolDownTimer -= Time.deltaTime;
        if (isAttackAble > 0f)
            isAttackAble -= Time.deltaTime;
        else
        {
            mySwordTrail.GetComponent<TrailRenderer>().emitting = false;
            mySwordHitbox.enabled = false;
        }
        if (sprintBoosting > 0f) sprintBoosting -= Time.deltaTime * 3;//순간 대쉬 속도 감소
        if (sprintBoostingCooldownTimer > 0f) sprintBoostingCooldownTimer -= Time.deltaTime;//순간 대쉬 딜레이 계산
    }

    void MoveWithDirection(bool rotateOnly = false)
    {
        //카메라 기준 앞 방향
        Vector3 camLook = (Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * Vector3.forward).normalized;
        float modelRotation = -90f;//오른쪽을 기준으로 몇도가 이미 돌아가 있는 모델인지
        float camRot = -Mathf.Atan2(camLook.z, camLook.x) * Mathf.Rad2Deg;//카메라가 위를 볼 때 0도 기준
        //키 입력 벡터
        Vector3 moveLook = new Vector3(InputManager.Instance.GetAxisMove().x, 0, InputManager.Instance.GetAxisMove().y).normalized;
        float moveRot = -((Mathf.Atan2(moveLook.z, moveLook.x) * Mathf.Rad2Deg) - 90f);//위 0도, 오른쪽 90도 기준
        //방향 설정 (키입력 0인경우 기존 방향 유지)
        if (Vector3.Distance(Vector3.zero, moveLook) > 0)
        {
            //카메라 회전값 방향에 윗키가 0도 기준으로 회전값을 합한 곳으로 회전
            var rotateAmount = camRot + moveRot;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotateAmount - modelRotation, 0), 360 * Time.deltaTime);
            if (rotateOnly == true) return;
            else isMove = true;
        }
        else isMove = false;
        //var move = Quaternion.Euler(0, camRot, 0) * moveLook;//누른 키 기준으로 즉시 이동
        var move = transform.rotation * (Vector3.forward * Vector3.Distance(Vector3.zero, moveLook));//현재 회전 값 기준으로 이동
        //이동 적용
        //달리기 누르고 있을 시 추가속도 적용
        if (isSprint)
        {
            var raw = (move * ud.Speed) * (sprintMultiplyer + sprintBoosting);
            rb.velocity = raw;
        }
        else
        {
            var raw = (move * ud.Speed);
            //Debug.Log(move);
            raw = new Vector3(raw.x, rb.velocity.y, raw.z);
            rb.velocity = raw;
            //Debug.Log(rb.velocity);
        }

    }
    public void Dead()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
        rb.isKinematic = true;
        col.enabled = false;
        mySwordHitbox.enabled = false;
        GameManager.Instance.OpenGameoverUI();
    }
    public void OnHpChanged()
    {
        var nowHp = ud.Hp;
        if (befHp > nowHp && takeDamegeAnimCoolDownTimer <= 0f)
        {
            if (befHp - nowHp != 1)
            {
                animator.SetTrigger("TriggerTakeDamage");
                takeDamegeAnimCoolDownTimer = takeDamageAnimCoolDown;
            }
        }
        befHp = nowHp;
        hpBar.fillAmount = ud.HpRatio;
        hpText.text = ud.Hp.ToString() + "/" + ud.MaxHp.ToString();
    }
    public void OnMpChanged()
    {
        mpBar.fillAmount = ud.MpRatio;
        mpText.text = ud.Mp.ToString() + "/" + ud.MaxMp.ToString();
    }
    public void OnExpChanged()
    {
        expBar.fillAmount = ud.ExpRatio;
        expText.text = ud.Exp.ToString() + "/" + ud.MaxExp.ToString();
    }
    public void OnLvChanged()
    {
        //레벨 설정
        lvText.text = ud.Lv.ToString();
        //레벨업 효과 발생
        GameManager.Instance.PlayerGetLevelUp();
        //체력 마력 회복
        ud.HpRatio = 1;
        ud.MpRatio = 1;
    }
}
