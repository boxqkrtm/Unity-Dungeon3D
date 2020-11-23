using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour
{
    Material m;
    MeshFilter mf = null;
    // GameObject afterImageObj = null;
    Coroutine fadeoutCoroutine = null;
    float originAlpha = 0f;
    public Mesh mesh { get { return mf.mesh; } }

    public void InitAfterImage(Material material)
    {
        // afterImageObj = new GameObject();
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();

        m = new Material(material);
        originAlpha = m.color.a;
        mr.material = m;
        mf = gameObject.AddComponent<MeshFilter>();

        gameObject.SetActive(false);
    }

    public void CreateAfterImage(Vector3 position, Quaternion rot, float time)
    {
        if (fadeoutCoroutine == null)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = position;
            var newRot = Quaternion.Euler(rot.eulerAngles.x  -89.98f, rot.eulerAngles.y, rot.eulerAngles.z);
            gameObject.transform.rotation = newRot;
            gameObject.GetComponent<MeshRenderer>().receiveShadows = false;
            gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            mf.mesh = mesh;
            fadeoutCoroutine = StartCoroutine(FadeOut(time));
        }
    }

    IEnumerator FadeOut(float time)
    {
        while (time > 0f)
        {
            time -= Time.deltaTime;
            m.color = new Color(m.color.r, m.color.g, m.color.b, originAlpha * time);
            yield return null;
        }

        gameObject.SetActive(false);
        fadeoutCoroutine = null;
    }
}
