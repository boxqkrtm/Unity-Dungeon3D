using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    private Material mat;

    void Start()
    {
        mat = gameObject.GetComponent<MeshRenderer>().material;
        Color newColor = mat.color;
        newColor.a = 0.2f;
        mat.color = newColor;
        gameObject.GetComponent<MeshRenderer>().material = mat;
        StartCoroutine(r());
    }

    IEnumerator r()
    {
        mat = gameObject.GetComponent<MeshRenderer>().material;

        Color newColor = mat.color;
        newColor.a = 0.2f;
        mat.color = newColor;
        gameObject.GetComponent<MeshRenderer>().material = mat;
        while (mat.color.a > 0.01f)
        {
            yield return null;
            newColor = mat.color;
            newColor.a -= 0.05f * Time.deltaTime;
            mat.color = newColor;
            gameObject.GetComponent<MeshRenderer>().material = mat;
        }
        Destroy(gameObject);
    }
}