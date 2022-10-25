using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPocket : MonoBehaviour
{
    //WeaponType weapon = WeaponType.PISTOL;
    private Animator weaponAnim;
    private SpriteRenderer weaponSprite;
    private Transform firePosition = null;

    private void Awake()
    {
        weaponAnim = GetComponent<Animator>();
        weaponSprite = GetComponentInChildren<SpriteRenderer>();
        firePosition = transform.GetChild(0).GetChild(0);
    }

    public void InitializeWeaponAnimator(int slotNumber)
    {
        weaponSprite.sprite =
            GameManager.Inst.Player.W_InvenUI.SlotUIs[slotNumber].Weapon_Slot.WeaponSlotData.weaponIcon;
        weaponAnim.runtimeAnimatorController =
            GameManager.Inst.Player.W_InvenUI.SlotUIs[slotNumber].Weapon_Slot.WeaponSlotData.weaponAniamtor;
    }


    //########## Setup
    public Vector2 SetupFirePosition() => firePosition.position;
    public Quaternion SetupFireRotation() => firePosition.rotation; 

    //########## Animation 
    public void PlayReloadAnimation()
    {
        weaponAnim.SetTrigger("onReload");
    }

    public void PlayFireAnimation()
    {
        weaponAnim.SetBool("isFiring", true);
    }

    public void PlayIdleAnimation()
    {
        weaponAnim.SetBool("isFiring", false);
    }
}
