using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Player player = null;
    public Player Player { get => player; }
    
    private PlayerControl control = null;
    public PlayerControl Control { get => control; }

    private WeaponDataManager weaponData;
    public WeaponDataManager Weapon_Data { get => weaponData; }

    private ItemDataManager itemDataManager;
    public ItemDataManager ItemDataManager => itemDataManager;


    protected override void Initialize()
    {
        player = FindObjectOfType<Player>();
        control = player.GetComponent<PlayerControl>();
        weaponData = GetComponent<WeaponDataManager>();
        itemDataManager = GetComponent<ItemDataManager>();
    }
}
