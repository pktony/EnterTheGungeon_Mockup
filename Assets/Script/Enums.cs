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
    ENEMY,
    CIRCLE,  // unlit
    BIG,
    MID, // Lit
    FOOTBALL,
    SPINNING,
    GOBLET
}

public enum EnemyID
{
    SHOTGUNKIN = 0,
    BULLETKIN
}

public enum FxID
{
    BLANKFX = 0
}

public enum DoorDirection : byte
{
    All = 0, North, South, East, West,
    UD, LR, UR, UL, DR, DL,
    ULR, DLR, UDL, UDR,
    Island
}