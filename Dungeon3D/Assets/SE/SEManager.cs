using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    public AudioClip buttonSE, fireSE, hitSE, iceSE, levelupSE, lightningSE, poisonSE, shotSE, takehitSE, despawnSE;
    public AudioClip sprintSE, closeSE;
    public AudioClip slashSE;
    public AudioClip useSE;
    public GameObject se;
    private static SEManager instance = null;
    public static SEManager Instance
    { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }
    
    AudioSource aso;

    private void Start()
    {
        aso = GetComponent<AudioSource>();
    }

    public void Play(AudioClip c)
    {
        var aso = Instantiate(se);
        aso.transform.position = GameManager.Instance.PlayerObj.transform.position;
        aso.GetComponent<AudioSource>().clip = c;
        aso.GetComponent<AudioSource>().Play();
    }
}
