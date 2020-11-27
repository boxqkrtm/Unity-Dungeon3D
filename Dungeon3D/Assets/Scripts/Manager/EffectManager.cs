using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    public Camera mainCamera;

    public GameObject hitDamageText;
    public GameObject hitSwordEffect;
    public GameObject mobDespawnEffect;
    public GameObject levelUpEffect;
    public GameObject hitFireEffect;
    public GameObject dashBoostEffect;
    public GameObject droppedItem;
    public GameObject poisonEffect;
    public GameObject lightningEffect;
    public GameObject fireEffect;
    public GameObject iceEffect;
    public GameObject lazerGroundEffect;

    float itemDropPower = 8f;

    private static EffectManager instance = null;
    public static EffectManager Instance
    { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }
    bool isStopEffect = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    float targetTimeScale = 1f;
    private void Update()
    {

        if (InventoryManager.Instance.IsInventoryOpened() == true
            || isStopEffect == true)
            targetTimeScale = 0.15f;
        else targetTimeScale = 1f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        Time.timeScale = Mathf.MoveTowards(Time.timeScale, targetTimeScale, 1f * Time.deltaTime * 10 / Time.timeScale);
    }
    public void CreateHitSwordEffect(Vector3 position)
    {
        Instantiate(hitSwordEffect, position, Quaternion.identity);
        SEManager.Instance.Play(SEManager.Instance.hitSE);
    }

    public void CreateMobDespawnEffect(Vector3 position)
    {
        Instantiate(mobDespawnEffect, position, Quaternion.identity);
    }

    public void CreateHitDamageText(Vector3 position, int damage, float effective = 0)
    {
        position.y += 2f;
        //position += (camera.transform.position - position).normalized * 5f;
        var go = Instantiate(hitDamageText, position, Quaternion.identity);
        go.GetComponent<HitDamageOverlayRemover>().SetText(damage.ToString());
        go.transform.localScale = Vector3.one * effective;
        //Debug.Log(effective);
        Camera.main.GetComponent<FollowPlayer>().StartShake(effective / 2);
        StopEffect();
    }

    public void CreateHitFireEffect(Vector3 pos)
    {
        Instantiate(hitFireEffect, pos, Quaternion.identity);
    }
    public void CreateLevelupEffect(Vector3 pos)
    {
        var a = Instantiate(levelUpEffect, pos, Quaternion.Euler(-90f, 0, 0));
        a.transform.SetParent(GameManager.Instance.PlayerObj.transform);
        SEManager.Instance.Play(SEManager.Instance.levelupSE);
    }

    public void CreateDashBoostEffect(Vector3 pos)
    {
        Instantiate(dashBoostEffect, pos, Quaternion.identity);
    }

    public void CreateDroppedItem(Vector3 pos, Item item)
    {
        var newpos = pos;
        newpos.y += 0.5f;
        var dr = Instantiate(droppedItem, newpos, Quaternion.identity);
        dr.GetComponent<DroppedItem>().SetItem(item);
        var power = Quaternion.Euler(Random.Range(-90f, 90f), Random.Range(0, 360f), 0) * Vector3.up * itemDropPower;
        dr.GetComponent<Rigidbody>().AddForce(power, ForceMode.VelocityChange);
        //Debug.Log(power);
    }

    public void CreatePoisonEffect(Vector3 pos)
    {
        Instantiate(poisonEffect, pos, Quaternion.identity);
    }

    public void CreateLightningEffect(Vector3 pos)
    {
        Instantiate(lightningEffect, pos, Quaternion.identity);
    }
    public void CreateFireEffect(Vector3 pos)
    {
        Instantiate(fireEffect, pos, Quaternion.identity);
    }
    public void CreateIceEffect(Vector3 pos)
    {
        Instantiate(iceEffect, pos, Quaternion.identity);
    }

    public void CreateLazerGroundEffect(Vector3 pos)
    {
        Instantiate(lazerGroundEffect, pos, Quaternion.identity);
    }


    public void StopEffect(float time = 0.09f)
    {
        StartCoroutine(StopEffectTimer(time));
    }

    IEnumerator StopEffectTimer(float time)
    {
        yield return new WaitForSeconds(0.1f);
        isStopEffect = true;
        float preTime = 0;
        while (preTime <= time)
        {
            yield return null;
            preTime += (1 / Time.timeScale) * Time.deltaTime;
            //print(preTime);
        }
        isStopEffect = false;
    }

    //일시적으로 상대에게 카메라를 옮김
    bool isCamLock = false;
    Vector3 locationDelta2Backup;
    float targetCamRotationEulerYBackup;
    public void LockCameraTargetEffect(Transform t, Vector3 locationDelta2, float targetCamRotationEulerY)
    {
        if (isCamLock == false)
        {
            InventoryManager.Instance.CloseInventory();
            isCamLock = true;

            mainCamera.GetComponent<FollowPlayer>().targetTransform = t;

            locationDelta2Backup = mainCamera.GetComponent<FollowPlayer>().locationDelta2;
            mainCamera.GetComponent<FollowPlayer>().locationDelta2 = locationDelta2;

            targetCamRotationEulerYBackup = mainCamera.GetComponent<FollowPlayer>().targetCamRotationEulerY;
            mainCamera.GetComponent<FollowPlayer>().targetCamRotationEulerY = targetCamRotationEulerY;
            mainCamera.GetComponent<FollowPlayer>().lockCameraControl = true;
        }
    }
    public void UnLockCameraTargetEffect()
    {
        if (isCamLock == true)
        {
            mainCamera.GetComponent<FollowPlayer>().targetTransform = GameManager.Instance.PlayerObj.transform;
            mainCamera.GetComponent<FollowPlayer>().locationDelta2 = locationDelta2Backup;
            mainCamera.GetComponent<FollowPlayer>().targetCamRotationEulerY = targetCamRotationEulerYBackup;
            isCamLock = false;
            mainCamera.GetComponent<FollowPlayer>().lockCameraControl = false;
        }
    }

    public void FadeIn()
    {
        GameObject.Find("Fade").GetComponent<Fade>().FadeOut();
    }

    public void FadeOut()
    {
        GameObject.Find("Fade").GetComponent<Fade>().FadeIn();
    }

}
