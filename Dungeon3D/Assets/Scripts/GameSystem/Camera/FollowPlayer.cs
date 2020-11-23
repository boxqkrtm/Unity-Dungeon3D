using System.Collections;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 targetPositionLate;

    Vector3 maxZoom = new Vector3(0, 4.38f, -6.37f);
    Vector3 minZoom = new Vector3(0, 11.39f, -20.29f);
    public Vector3 locationDelta2 = new Vector3(0, 10.45f, -10f);
    Vector3 locationDelta2Late;

    public Vector3 locationDelta = new Vector3(0, 0, 0);
    public float targetCamRotationEulerY = 0f;
    public float targetCamRotationEulerX = 0f;
    public float maxYSpinAround = 25f;
    public bool lockCameraControl = false;
    private float sensitivity = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        lockCameraControl = false;
        targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        targetPositionLate = targetTransform.position;
        locationDelta2Late = locationDelta2;
    }

    // Update is called once per frame
    void Update()
    {
        #region 카메라 마우스 제어
        if (lockCameraControl == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0 && InventoryManager.Instance.IsInventoryOpened() == false)
            {
                float wheel = Input.GetAxis("Mouse ScrollWheel");
                if (wheel > 0)
                {
                    //zoom in
                    var temp = maxZoom;
                    temp.y = transform.position.y;
                    locationDelta2 = Vector3.MoveTowards(locationDelta2, temp, 1f);
                    //Debug.Log(locationDelta2);
                }
                else
                {
                    //zoom out
                    var temp = minZoom;
                    temp.y = transform.position.y;
                    locationDelta2 = Vector3.MoveTowards(locationDelta2, temp, 1f);
                }
            }
            if ((Input.GetMouseButton(1) || Input.GetMouseButton(0)) && InventoryManager.Instance.IsInventoryOpened() == false)
            {
                var rawMove = 900 * Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
                if (Mathf.Abs(rawMove) > maxYSpinAround)
                {
                    //최대 25도 이동가능
                    rawMove = rawMove / Mathf.Abs(rawMove) * maxYSpinAround;
                }
                targetCamRotationEulerY += rawMove;

                rawMove = -90 * Input.GetAxis("Mouse Y") * Time.deltaTime;
                locationDelta2.y += rawMove;
                if (locationDelta2.y < maxZoom.y) locationDelta2.y = maxZoom.y;
                if (locationDelta2.y > minZoom.y) locationDelta2.y = minZoom.y;
            }
        }
        #endregion
        transform.position += locationDelta;
    }

    void FixedUpdate()
    {
        //targetCamRotationEulerY의 범위를 0~360으로 맞춤
        targetCamRotationEulerY = ZeroTo360(targetCamRotationEulerY);
        var nowCamRot = transform.rotation.eulerAngles.y;
        var smoothRot = nowCamRot;
        //원 궤도에 따라서 부드럽게 움직임
        var rotDelta = Mathf.Abs(nowCamRot - targetCamRotationEulerY);
        if (nowCamRot > targetCamRotationEulerY)
        //359->2 (need 359->360)
        {

            if (rotDelta < 180f)
                smoothRot = Mathf.Lerp(nowCamRot, targetCamRotationEulerY, 6f * Time.fixedDeltaTime);
            else
                smoothRot = Mathf.Lerp(nowCamRot, targetCamRotationEulerY + 360f, 5f * Time.fixedDeltaTime);
        }
        else if (nowCamRot < targetCamRotationEulerY)
        //2->359 (need 362->359)
        //360.1 -> 0
        {
            if (rotDelta < 180f)
                smoothRot = Mathf.Lerp(nowCamRot, targetCamRotationEulerY, 6f * Time.deltaTime);
            else
                smoothRot = Mathf.Lerp(nowCamRot + 360f, targetCamRotationEulerY, 6f * Time.fixedDeltaTime);
        }
        if (targetTransform != null)
        {
            targetPositionLate = Vector3.Lerp(targetPositionLate, targetTransform.position, 5f * Time.fixedDeltaTime);
        }
        var newPos = GetCirclePos(smoothRot);
        //transform.position = newPos + locationDelta;
        transform.position = newPos;
        transform.LookAt(targetPositionLate);

    }

    float ZeroTo360(float x)
    {
        var y = x < 0 ? x + 360f : x;
        y = y > 360 ? y - 360f : y;
        return y;
    }

    Vector3 GetCirclePos(float degree)
    {
        var camRotation = Quaternion.Euler(0, degree, 0);
        Vector3 camLocation = new Vector3(targetPositionLate.x, targetPositionLate.y, targetPositionLate.z);
        //캐릭터를 중심으로 원을 그린 후 그 위에 카메라를 움직임, locationDelta2는 카메라의 높이와 거리
        locationDelta2Late = Vector3.Lerp(locationDelta2Late, locationDelta2, 5f * Time.fixedDeltaTime);
        camLocation += camRotation * locationDelta2Late;
        camLocation.y = Mathf.Lerp(transform.position.y, camLocation.y, 5f * Time.fixedDeltaTime);
        return camLocation;
    }
    public void StartShake(float shakePower)
    {
        StartCoroutine(Shake(shakePower));
    }

    IEnumerator Shake(float shakePower = 0.4f)
    {
        for (var i = 0; i < 2; i++)
        {
            yield return null;
            locationDelta = new Vector3(Random.Range(-shakePower, shakePower), Random.Range(-shakePower, shakePower), Random.Range(-shakePower, shakePower));
        }
        locationDelta = Vector3.zero;
    }
}
