using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInventoryUI : MonoBehaviour
{
    ItemInventory itemInven;

    List<Image> blankShellImg = new();
    Image blankShell;
    TextMeshProUGUI keyCountText;
    TextMeshProUGUI shellCountText;

    public GameObject blankShellPrefab;

    private void Awake()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            blankShell = transform.GetChild(0).GetChild(i).GetComponent<Image>();
            blankShellImg.Add(blankShell);
        }
        //blankShell = transform.GetChild(0).GetComponentsInChildren<Image>();
        keyCountText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        shellCountText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();

        //Player player = FindObjectOfType<Player>();
        //itemInven = player.Inven_Item;

        //foreach (ItemSlot slot in itemInven.Slots)
        //{
        //    slot.onItemLoot += RefreshUIs;
        //}
        
    }

    private void Start()
    {
        //Player player = FindObjectOfType<Player>();
        itemInven = GameManager.Inst.Player.Inven_Item;

        foreach (ItemSlot slot in itemInven.Slots)
        {
            slot.onItemLoot += RefreshUIs;
        }

        RefreshUIs();
    }

    public void RefreshUIs()
    {
        keyCountText.text = itemInven.Slots[(uint)ItemType.Key].StackCount.ToString();
        shellCountText.text = (itemInven.Slots[(uint)ItemType.GoldShell].StackCount +
            itemInven.Slots[(uint)ItemType.SilverShell].StackCount +
            itemInven.Slots[(uint)ItemType.BronzeShell].StackCount).ToString();


        // ------ Blank Shell
        foreach (var shell in blankShellImg)
        {
            shell.color = Color.clear;
        }
        for (int i = 0; i < GameManager.Inst.Player.Inven_Item.Slots[(int)ItemType.BlankShell].StackCount; i++)
        {
            blankShellImg[i].color = Color.white;
        }
    }
}
