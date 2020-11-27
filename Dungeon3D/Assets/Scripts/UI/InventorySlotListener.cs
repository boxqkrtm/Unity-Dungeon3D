using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotListener : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public RectTransform windowParent;
    public Canvas canvas;
    public int myIndex;
    int mouseButton = -1;
    private Vector3 initLocation;
    public bool isDragAble = true;
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            InventoryManager.Instance.UseItem(myIndex);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragAble == false) return;
        InventoryManager.Instance.floatItem.SetActive(true);
        InventoryManager.Instance.floatItem.transform.GetChild(0).gameObject.SetActive(false);
        InventoryManager.Instance.floatItem.GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<Image>().sprite;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        if (myIndex > -1 && InventoryManager.Instance.PlayerItems[myIndex].ItemAmount > 1)
        {
            InventoryManager.Instance.floatItem.transform.GetChild(0).gameObject.SetActive(true);
            InventoryManager.Instance.floatItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            InventoryManager.Instance.PlayerItems[myIndex].ItemAmount.ToString();
        }
        InventoryManager.Instance.floatItem.GetComponent<RectTransform>().position = Input.mousePosition;
        if (Input.GetMouseButton(0)) mouseButton = 0;
        if (Input.GetMouseButton(1)) mouseButton = 1;
        if (mouseButton == 1)
        {
            var fulllAmount = InventoryManager.Instance.PlayerItems[myIndex].ItemAmount;
            if (fulllAmount == 1)
            {
                mouseButton = 0;
                return;
            }
            var halfAmount1 = (fulllAmount / 2);//옮겨질 아이템 수량
            var halfAmount2 = fulllAmount - halfAmount1;//기존 아이템 수량
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            InventoryManager.Instance.floatItem.transform.GetChild(0).gameObject.SetActive(true);
            InventoryManager.Instance.floatItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = halfAmount1.ToString();
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = halfAmount2.ToString();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragAble == false) return;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        InventoryManager.Instance.floatItem.SetActive(false);
        windowParent.anchoredPosition = initLocation;
        var results = new List<RaycastResult>();
        var graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        graphicRaycaster.Raycast(eventData, results);
        // Check all hits.
        var targetIndex = -1;
        var isOuterInvUI = true;
        foreach (var hit in results)
        {
            isOuterInvUI = false;
            var target = hit.gameObject.GetComponent<InventorySlotListener>();
            if (target != null)
            {
                targetIndex = target.myIndex;
                break;
            }
        }
        if (mouseButton == 0)
        {
            if (isOuterInvUI == true)
                InventoryManager.Instance.DropItem(myIndex);
            if (targetIndex != -1)
                InventoryManager.Instance.SwapItem(myIndex, targetIndex);
        }
        else if (mouseButton == 1)
        {

            if (targetIndex != -1)
                InventoryManager.Instance.SplitItem(myIndex, targetIndex);
        }
        InventoryManager.Instance.UIUpdate();
    }

    // Start is called before the first frame update
    void Start()
    {
        windowParent = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        initLocation = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragAble == false) return;
        InventoryManager.Instance.HideItemInfo();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragAble == false) return;
        InventoryManager.Instance.ShowItemInfo(myIndex);
    }
}
