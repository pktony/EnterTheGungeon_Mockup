using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bullet_UI : MonoBehaviour
{
    // ##################### Components ########################
    TextMeshProUGUI bulletText;
    [SerializeField] private Transform parent;
    Image[] bulletImg;

    [SerializeField] private GameObject bulletImage;
    WeaponData weapon;

    private void Awake()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(bulletImage, parent);
        }

        bulletImg = GetComponentsInChildren<Image>();
        bulletText = GetComponentInChildren<TextMeshProUGUI>();

    }

    private void Start()
    {
        //InitializeBullet_UI();
        GameManager.Inst.Player.onFireReload = RefreshBullet_UI;
    }

    public void InitializeBullet_UI()
    {
        weapon = GameManager.Inst.Player.CurrentWeapon;
        for (int i = 0; i < weapon.maxBulletMagazine; i++)
        {
            bulletImg[i].sprite = weapon.BulletIcon;
            bulletImg[i].color = Color.white;
        }
        RefreshBullet_UI();
    }

    public void RefreshBullet_UI()
    {
        weapon = GameManager.Inst.Player.CurrentWeapon;
        int bulletInMag = GameManager.Inst.Player.BulletinMag;
        
        for (int i = 0; i < bulletInMag ; i++)
        {
            bulletImg[i].color = Color.white;
        }

        for (int i = bulletInMag; i < bulletImg.Length; i++)
        {
            bulletImg[i].color = Color.clear;
        }

        if (weapon.id > 0)
        {
            bulletText.text = $"{GameManager.Inst.Player.CurrentWeapon.remainingBullet} / {weapon.maxBulletNum}";
        }
        else
        {
            bulletText.text = "Infinite";
        }
    }
}
