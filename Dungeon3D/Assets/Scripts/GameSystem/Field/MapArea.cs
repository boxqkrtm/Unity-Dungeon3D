using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MapArea : MonoBehaviour
{
    public Vector2Int safeLevel;//권장 레벨
    public GameObject spawnMob;
    public int spawnMobMaximum = 1;
    public float spawnMobDelay = 5f;
    List<Transform> pos;
    List<GameObject> mob;
    // Start is called before the first frame update
    void Start()
    {
        pos = new List<Transform>();
        mob = new List<GameObject>();
        foreach (Transform i in transform)
        {
            pos.Add(i);
        }
        StartCoroutine(SpawnMobLoop());
    }

    IEnumerator SpawnMobLoop()
    {
        var posIndex = 0;
        while (true)
        {
            if (!(posIndex < pos.Count)) posIndex = 0;
            yield return new WaitForSeconds(1f);//과부하 방지
            if (pos.Count == 0) break;
            for (var i = 0; i < mob.Count; i++)
            {
                if (mob[i] == null)
                {
                    mob.RemoveAt(i);
                }
            }

            if (mob.Count < spawnMobMaximum)
            {
                var selectedPos = pos[posIndex++];
                var distance = Vector3.Distance(GameManager.Instance.PlayerObj.transform.position,
                selectedPos.position);

                //플레이어 근처 혹은 몹끼리 겹침스폰 방지
                var isNeedContinue = false;
                if (distance <= 10f) isNeedContinue = true;
                foreach (var m in mob)
                {
                    if (m == null) continue;
                    Vector3 x = new Vector3(m.transform.position.x, 0, m.transform.position.z);
                    Vector3 y = new Vector3(selectedPos.transform.position.x, 0, selectedPos.transform.position.z);
                    if (Vector3.Distance(x, y) < 5f)
                        isNeedContinue = true;
                }
                if (isNeedContinue) continue;

                var spawnedMob = Instantiate(spawnMob, selectedPos.position, Quaternion.identity);
                mob.Add(spawnedMob);
                //spawn mob
            }
            //else skip
            yield return new WaitForSeconds(spawnMobDelay);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AlertManager.Instance.AlertNowArea(gameObject.name, safeLevel);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
