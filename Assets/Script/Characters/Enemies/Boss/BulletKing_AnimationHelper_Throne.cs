using UnityEngine;

/// <summary>
/// 애니메이션 타이밍 도움을 주기 위한 클래스  
/// </summary>
public class BulletKing_AnimationHelper_Throne : MonoBehaviour
{
    public System.Action onSpinAttack_Even;
    public System.Action onSpinAttack_Odd;

    public void SpinAttack_Even() => onSpinAttack_Even?.Invoke();
    public void SpinAttack_Odd() => onSpinAttack_Odd?.Invoke();
}
