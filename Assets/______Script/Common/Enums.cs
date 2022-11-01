using System.Collections;
using System.Collections.Generic;

public enum PlayerMove
{
    IDLE = 0,
    WALK,
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

public enum ItemType
{
    BlankShell = 0,
    AmmoBox,
    Key,
    Heart,
    GoldShell,
    SilverShell,
    BronzeShell
}

public enum BulletType
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


#region Sound Clips

public enum Clips_UI : byte
{
    Title_Intro = 0,
    Menu_Confirm,
    Menu_Pause,
    Menu_Select,
    Menu_Cancel
}

public enum Clips_Player : byte
{
    blank = 0,
    Footstep1,
    Footstep2,
    Footstep3
}

public enum Clips_Weapon : byte
{
    AK47_Shot1 = 0,
    AK47_Shot2,
    AK47_Shot3,
    AK47_Reload,

    Deagle_Shot1,
    Deagle_Shot2,
    Deagle_Shot3,
    Deagle_Reload,

    Shotgun_Shot1,
    Shotgun_Shot2,
    Shotgun_Shot3,

    Colt_Shot1,
    Colt_Shot2,
    Colt_Shot3,
}

public enum Clips_ShotgunKin : byte
{
    Shotgun_Hurt1 = 0,
    Shotgun_Hurt2,
    Shotgun_Death1,
    Shotgun_Death2,
}

public enum Clips_BulletKin : byte
{
    BulletKin_Hurt1 = 0,
    BulletKin_Hurt2,
    BulletKin_Death1,
    BulletKin_Death2,
}

public enum Clips_Boss : byte
{
    Boss_Shot0,
    Boss_Shot1,
    Boss_Shot2,
    Boss_Spin,
    Boss_Throw,
    Boss_Wine,
    Boss_Explode,
    Boss_Intro
}

public enum Clips_Item : byte
{
    AmmoPickup = 0,
    Coin_Large,
    Coin_Medium,
    Coin_Small,
    Heart,
    Item_Throw,
    Key_Pickup,
    Teleport_Activate,
    Teleport_Depart,
    Teleport_Arrive,
    Chest_Unlock
}
#endregion