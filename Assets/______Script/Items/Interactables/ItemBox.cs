using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : Item
{
    Animator anim;
    private float throwDuration = 0.5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public override void LootAction()
    {
        if (player.Inven_Item[(int)ItemID.Key].StackCount > 0)
        {
            anim.SetTrigger("onLoot");
            player.Inven_Item[(int)ItemID.Key].StackCount--;
            // 50대 50확률로 아이템 드랍 
            float rand = Random.value;
            GameObject obj = rand < 0.5f ?
                ItemManager.Inst.GetPooledItem(ItemID.AmmoBox) :
                ItemManager.Inst.GetPooledItem(ItemID.Heart);
            obj.transform.position =
                (Vector2)transform.position + Vector2.down;
            StartCoroutine(ThrowItem(obj));
            obj.SetActive(true);
        }
    }

    IEnumerator ThrowItem(GameObject obj)
    {
        float timer = 0f;
        while(timer < throwDuration)
        {
            obj.transform.Translate(Time.deltaTime * Vector3.down);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
