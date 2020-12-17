using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Npc : MonoBehaviour
{
    virtual protected int NpcData { get; set; }//override로 npc 정보 할당
    protected int tempNpcData;
    protected NpcManager nmgr = NpcManager.Instance;
    protected GameManager gmgr = GameManager.Instance;
    public List<NpcTalkNode> talks;
    PlayerControl ps;
    float attackKey = 0f;
    public float targetRotation = 30f;
    float modelRotation = -90f;
    void Start()
    {
        nmgr = NpcManager.Instance;
        gmgr = GameManager.Instance;
        tempNpcData = NpcData;
        attackKey = 0f;
        ps = new PlayerControl();
        ps.Play.Enable();
        ps.Play.Attack.performed += AttackBtn;
        StartCoroutine(IdleEvent());
    }

    // Update is called once per frame
    void Update()
    {
        if (attackKey > 0) attackKey -= Time.deltaTime;
        if (attackKey < 0)
        {
            attackKey += Time.deltaTime;
            if (attackKey > 0) attackKey = 0;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotation - modelRotation, 0), 2 * Time.deltaTime);
        if (isTalking == true) TalkingLoop();
    }

    void TalkingLoop()
    {
        var result = talks[tempNpcData].Execute(nmgr.OptionNumber);
        if (result.nextState >= 0)
        {
            SEManager.Instance.Play(SEManager.Instance.buttonSE);
            tempNpcData = result.nextState;
        }
        else if (result.nextState <= -2)
        {
            //상태를 변경하지 않고 임의 함수를 실행
            TalkingCustomFunction(result.nextState);
        }
        if (result.isLast == true)
        {
            //대화가 성공적으로 종료 된 경우에만 NPC 대화 데이터 저장
            NpcData = tempNpcData;
            TalkingStop();
        }
    }

    //state -2부터 -n (n은 양의 정수)까지 임의 함수 시작
    virtual protected void TalkingCustomFunction(int state) { }

    void LookPlayer()
    {
        var targetDirectVector = GameManager.Instance.PlayerObj.transform.position - transform.position;
        targetRotation = -Mathf.Atan2(targetDirectVector.z, targetDirectVector.x) * Mathf.Rad2Deg;
    }

    void LookRandom()
    {
        targetRotation = Random.Range(90, 180f);
    }

    void AttackBtn(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if(InventoryManager.Instance.IsInventoryOpened() == false)
            attackKey += 0.1f;
    }
    bool isTalking;

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (attackKey > 0f && isTalking == false)
            {
                TalkingStart();
            }
        }
    }

    IEnumerator IdleEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (isTalking == false)
            {
                LookRandom();
            }
        }
    }

    protected void TalkingStart()
    {
        isTalking = true;
        InputManager.Instance.InputStop();
        EffectManager.Instance.LockCameraTargetEffect(transform, new Vector3(0, 4.38f, -6.37f), 18f);
        LookPlayer();//플레이어를 바라봄
        SEManager.Instance.Play(SEManager.Instance.buttonSE);
    }

    protected void TalkingStop()
    {
        isTalking = false;
        InputManager.Instance.InputStart();
        EffectManager.Instance.UnLockCameraTargetEffect();
        NpcManager.Instance.CloseInteractWindow();
        SEManager.Instance.Play(SEManager.Instance.closeSE);
        attackKey = -1f;
    }

}
