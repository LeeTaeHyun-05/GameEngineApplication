using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotInventory : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemText;

    public void ItemSetting(Sprite itemSprite, string txt)
    {
        itemImage.sprite = itemSprite;
        itemText.text = txt;
    }

   
}
