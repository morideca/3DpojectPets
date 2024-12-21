using UnityEngine;

public class DamageManager : MonoBehaviour
{
    private ServiceLocator serviceLocator;
    private EventManager eventManager;

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
    }

    private void OnEnable()
    {
        if (eventManager == null) eventManager = serviceLocator.EventManager;
        eventManager.OnPlayerAttackEnemy += DealDamage;
        eventManager.OnMonsterAttackPet += DealDamage;
    }

    private void OnDisable()
    {
        eventManager.OnPlayerAttackEnemy -= DealDamage;
        eventManager.OnMonsterAttackPet -= DealDamage;
    }

    private void DealDamage(GameObject target, int amount)
    {
        target.TryGetComponent<IDamageable>(out var _target);
        if (_target != null) _target.GetDamage(amount);
    }
}
