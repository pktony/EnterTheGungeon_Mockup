using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoItems : MonoBehaviour
{
    Player player;

    uint bronze;
    uint silver;
    uint gold;

    uint bronzeID;
    uint silverID;
    uint goldID;    

    private void Awake()
    {
        player = GameManager.Inst.Player;

        gold = GameManager.Inst.ItemDataManager.itemDatas[(int)ItemID.GoldShell].value;
        silver = GameManager.Inst.ItemDataManager.itemDatas[(int)ItemID.SilverShell].value;
        bronze = GameManager.Inst.ItemDataManager.itemDatas[(int)ItemID.BronzeShell].value;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("BronzeShell"))
            {
                player.Inven_Item.Slots[(int)ItemID.GoldShell].IncreaseItem(bronze);
                ItemManager.Inst.ReturnItem(ItemManager.PooledItems[(uint)ItemID.BronzeShell], this.gameObject);
            }
            else if (this.gameObject.CompareTag("SilverShell"))
            {
                player.Inven_Item.Slots[(int)ItemID.GoldShell].IncreaseItem(silver);
                ItemManager.Inst.ReturnItem(ItemManager.PooledItems[(uint)ItemID.SilverShell], this.gameObject);
            }
            else
            {
                player.Inven_Item.Slots[(int)ItemID.GoldShell].IncreaseItem(gold);
                ItemManager.Inst.ReturnItem(ItemManager.PooledItems[(uint)ItemID.GoldShell], this.gameObject);
            }
        }
    }
}
