using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Item
{
    Animator anim;
    GameObject shadow;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        shadow = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        shadow.SetActive(true);
    }

    public override void LootAction()
    {
        anim.SetTrigger("onLoot");
        shadow.SetActive(false);
        player.CurrentWeapon.bulletsInPocket = player.CurrentWeapon.maxBulletNum;
        player.W_InvenUI.BulletUI.RefreshBullet_UI();
        SoundManager.Inst.PlaySound_Item(Clips_Item.AmmoPickup);
        StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        float time = anim.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(time);
        ItemManager.Inst.ReturnItem(ItemType.AmmoBox, this.gameObject);
    }
}
