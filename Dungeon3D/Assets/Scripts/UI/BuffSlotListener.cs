using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuffSlotListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public string infoTxt;
    public void OnPointerEnter(PointerEventData eventData)
    {
        BuffManager.Instance.OpenBuffInfo(infoTxt, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BuffManager.Instance.CloseBuffInfo();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //GameManager.Instance.OpenBuffInfoMove(infoTxt, eventData.position);
    }
}
