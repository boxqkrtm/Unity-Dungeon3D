using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public Vector3 start;
    public Vector3 target;
    LineRenderer l;
    // Start is called before the first frame update
    private void Start()
    {
        start = Vector3.zero;
        target = Vector3.zero;
        l = GetComponent<LineRenderer>();

    }
    // Update is called once per frame
    void Update()
    {
        l.SetPositions(new Vector3[] { start, target });
    }
}
