using UnityEngine;

public class DamageManager : MonoBehaviour
{
    private void OnEnable()
    {
        UnitAttack.hitTarget += DealDamage;
        MonsterAI.monsterAttackedPet += DealDamage;
    }

    private void OnDisable()
    {
        UnitAttack.hitTarget -= DealDamage;
        MonsterAI.monsterAttackedPet -= DealDamage;
    }

    private void DealDamage(GameObject target, int amount)
    {
        target.TryGetComponent<IDamageable>(out var _target);
        if (_target != null) _target.GetDamage(amount);
    }
}
