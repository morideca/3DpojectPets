using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    private void OnEnable()
    {
        AttackManager.hitTarget += DealDamage;
        MonsterAI.monsterAttackedPet += DealDamage;
    }

    private void OnDisable()
    {
        AttackManager.hitTarget -= DealDamage;
        MonsterAI.monsterAttackedPet -= DealDamage;
    }

    private void DealDamage(GameObject target, int amount)
    {
        target.TryGetComponent<HealthManager>(out var _target);
        if (_target != null) target.GetComponent<HealthManager>().GetDamage(amount);
    }
}
