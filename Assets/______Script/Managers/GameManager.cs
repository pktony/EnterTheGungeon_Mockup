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

    private SoundManager soundManager;
    public SoundManager SoundManager => soundManager;

    #region Audio Variables
    private float volume_Master = 1f;
    private float volume_VFX = 1f;
    private float volume_UI = 1f;

    public float Volume_Master { get => volume_Master; set => volume_Master = value; }
    public float Volume_VFX { get => volume_VFX; set => volume_VFX = value; }
    public float Volume_UI { get => volume_UI; set => volume_UI = value; }
    #endregion

    protected override void Initialize()
    {
        player = FindObjectOfType<Player>();
        control = player.GetComponent<PlayerControl>();
        weaponData = GetComponent<WeaponDataManager>();
        itemDataManager = GetComponent<ItemDataManager>();
        soundManager = GetComponent<SoundManager>();
    }
}
