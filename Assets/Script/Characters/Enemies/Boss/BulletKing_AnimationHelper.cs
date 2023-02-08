using UnityEngine;

/// <summary>
/// 애니메이션 타이밍 도움을 주기 위한 클래스 
/// </summary>
public class BulletKing_AnimationHelper : MonoBehaviour
{
    public System.Action onTell2Attack;
    public System.Action onTell1Attack;
    public System.Action onTell3Attack;
    public System.Action onGobletAttack;

    public void AttackTell1() => onTell1Attack?.Invoke();
    public void AttackTell2() => onTell2Attack?.Invoke();
    public void AttackTell3() => onTell3Attack?.Invoke();
    public void ThrowGoblet() => onGobletAttack?.Invoke();
}
