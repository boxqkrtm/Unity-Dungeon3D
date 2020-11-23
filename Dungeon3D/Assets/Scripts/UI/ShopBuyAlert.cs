using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyAlert : MonoBehaviour
{
    public Text mainText;
    public Text amountAndPriceText;
    public Slider amountSlider;
    int amount = 1;
    Item buyItem;
    public void OpenAlert(Item item)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        buyItem = new Item(item);
        amount = 1;
        amountSlider.value = 1;
        mainText.text = item.ItemName + "을 구매하시겠습니까?";
        SliderOnChange();
    }
    public void SliderOnChange()
    {
        amount = (int)amountSlider.value;
        buyItem.ItemAmount = amount;
        var totalPrice = ((int)amountSlider.value * buyItem.ItemPrice);
        amountAndPriceText.text = amount.ToString() + "개" + " " + totalPrice.ToString() + "G";
        amountAndPriceText.color = Color.white;

        //구매 조건 미달 시 색 변경 및 표시
        if (InventoryManager.Instance.PlayerScript.ud.Gold < totalPrice)
        {
            amountAndPriceText.text += "(GOLD 부족)";
            amountAndPriceText.color = Color.red;
        }
        if (!InventoryManager.Instance.TestGetItem(buyItem))
        {
            amountAndPriceText.text += "(인벤토리 부족)";
            amountAndPriceText.color = Color.red;
        }
    }
    public void Buy()
    {
        var totalPrice = ((int)amountSlider.value * buyItem.ItemPrice);
        if (InventoryManager.Instance.PlayerScript.ud.Gold >= totalPrice)
        {
            if (InventoryManager.Instance.TestGetItem(buyItem))
            {

                AlertManager.Instance.AddGameLog("구입 성공");
                InventoryManager.Instance.PlayerScript.ud.Gold -= totalPrice;
                InventoryManager.Instance.GetItem(buyItem);
                transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                AlertManager.Instance.AddGameLog("인벤토리 공간이 부족합니다.");
            }
        }
        else
        {
            AlertManager.Instance.AddGameLog("GOLD가 부족합니다.");
        }
    }
    public void Cancel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
