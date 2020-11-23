using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float time = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
