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
    HAND = 0,
    PISTOL
}
