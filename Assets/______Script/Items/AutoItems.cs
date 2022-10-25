using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoItems : MonoBehaviour
{
    public ItemData_Shells data;

    uint value;

    Player player;

    private void Awake()
    {
        value = data.value;
    }

    private void Start()
    {
        player = GameManager.Inst.Player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(value);
            player.Inven_Item[(int)data.itemID].IncreaseItem(value);
            SoundManager.Inst.PlaySound_Item((Clips_Item)(data.itemID - 3));
            
            ItemManager.Inst.ReturnItem((ItemID)data.itemID, this.gameObject);
        }
    }
}
