using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    private void OnEnable()
    {
        AttackManager.hittedTarget += DealDamage;
        MonsterAI.monsterAttackedPet += DealDamage;
    }

    private void OnDisable()
    {
        AttackManager.hittedTarget -= DealDamage;
        MonsterAI.monsterAttackedPet -= DealDamage;
    }

    private void DealDamage(GameObject target, int amount)
    {
        target.GetComponent<HealthManager>().GetDamage(amount);
    }
}
