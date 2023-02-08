using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bullet_UI : MonoBehaviour
{
    Player player;
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
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        //InitializeBullet_UI();
        player.onFireReload += RefreshBullet_UI;
    }

    public void InitializeBullet_UI()
    {
        weapon = GameManager.Inst.Player.CurrentWeapon;
        for (int i = 0; i < weapon.bulletPerMagazine; i++)
        {
            bulletImg[i].sprite = weapon.BulletIcon;
            bulletImg[i].color = Color.white;
        }
        RefreshBullet_UI();
    }

    public void RefreshBullet_UI()
    {
        weapon = GameManager.Inst.Player.CurrentWeapon;

        int bulletsInMagazine = GameManager.Inst.Player.BulletInMag;

        for (int i = 0; i < bulletsInMagazine ; i++)
        {
            bulletImg[i].color = Color.white;
        }

        for (int i = bulletsInMagazine; i < bulletImg.Length; i++)
        {
            bulletImg[i].color = Color.clear;
        }

        bulletText.text = weapon.id > 0 ?
            $"{GameManager.Inst.Player.CurrentWeapon.bulletsInPocket} / {weapon.maxBulletNum}"
            : "Infinite";
    }
}
