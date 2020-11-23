using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerMinimap : MonoBehaviour
{
    Transform playerTransform;
    Vector3 locationDelta = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 camLocation = new Vector3(playerTransform.position.x, playerTransform.position.y + 50.43f, playerTransform.position.z);
        transform.position = camLocation + locationDelta;
    }
}
