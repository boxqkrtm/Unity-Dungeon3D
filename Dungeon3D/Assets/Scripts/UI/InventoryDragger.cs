using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDragger : MonoBehaviour, IDragHandler
{
    public RectTransform windowParent;
    public Canvas canvas;
    public void OnDrag(PointerEventData eventData)
    {
        windowParent.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
