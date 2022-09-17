using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInventoryUI : MonoBehaviour
{
    ItemInventory itemInven;

    Image[] blankShell;
    TextMeshProUGUI keyCountText;
    TextMeshProUGUI shellCountText;


    private void Awake()
    {
        blankShell = transform.GetChild(0).GetComponentsInChildren<Image>();
        keyCountText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        shellCountText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        itemInven = GameManager.Inst.Player.Inven_Item;

        foreach (ItemSlot slot in itemInven.Slots)
        {
            slot.onItemLoot += RefreshUIs;
        }

        RefreshUIs();
    }

    public void RefreshUIs()
    {
        keyCountText.text = itemInven.Slots[(uint)ItemID.Key].StackCount.ToString();
        shellCountText.text = itemInven.Slots[(uint)ItemID.GoldShell].StackCount.ToString();


        foreach(var shell in blankShell)
        {
            shell.color = Color.clear;
        }
        for (int i = 0; i < GameManager.Inst.Player.Inven_Item.Slots[(int)ItemID.BlankShell].StackCount; i++)
        {
            blankShell[i].color = Color.white;
        }
    }
}
