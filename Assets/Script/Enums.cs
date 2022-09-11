using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMove
{
    IDLE = 0,
    WALK,
    RUN,
    DODGE
}

public enum EnemyState
{
    IDLE = 0,
    PATROL,
    TRACK,
    ATTACK,
    DEAD
}

public enum WeaponType
{
    PISTOL = 0,
    RIFLE
}

public enum ItemID
{
    BlankShell = 0,
    AmmoBox,
    Key,
    Heart,
    GoldShell,
    SilverShell,
    BronzeShell
}

public enum BulletID
{
    PLAYER = 0,
    ENEMY = 1,
    CIRCLE,
    SPEAR
}

public enum EnemyID
{
    SHOTGUNKIN = 0,
    BULLETKIN
}

public enum FxID
{
    BULLETEXPLOSION = 0,
    BLANKFX
}