using UnityEngine;
public interface IBattle
{
    void Attack(IBattle target);
    public void TakeDamage(int damage);
}