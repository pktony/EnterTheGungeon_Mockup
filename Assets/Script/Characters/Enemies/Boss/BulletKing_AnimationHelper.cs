using UnityEngine;

public class BulletKing_AnimationHelper : MonoBehaviour
{
    public System.Action onTell2Attack;
    public System.Action onTell1Attack;
    public System.Action onGobletAttack;

    public void AttackTell1() => onTell1Attack?.Invoke();
    public void AttackTell2() => onTell2Attack?.Invoke();
    public void ThrowGoblet() => onGobletAttack?.Invoke();
}
