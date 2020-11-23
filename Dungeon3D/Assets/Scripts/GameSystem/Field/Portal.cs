using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string TargetSceneName;
    public Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            GameManager.Instance.MoveScene(TargetSceneName, targetPosition);
        }
    }
}
