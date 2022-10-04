using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletKing_AnimationHelper_Throne : MonoBehaviour
{
    public Action onSpinAttack_Even;
    public Action onSpinAttack_Odd;

    public void SpinAttack_Even() => onSpinAttack_Even?.Invoke();
    public void SpinAttack_Odd() => onSpinAttack_Odd?.Invoke();
}
