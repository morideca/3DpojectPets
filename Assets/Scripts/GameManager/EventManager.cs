using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private ServiceLocator serviceLocator;

    public event Action OnPlayerBecameMainCharacter;
    public event Action OnPetBecameMainCharacter;
    public event Action<GameObject> OnPetReturnedToBall;
    public event Action<GameObject> OnPetSummoned;
    public event Action OnPetDeath;
    public event Action OnEnemyDeath;
    public event Action OnMonsterTouchPlayer;
    public event Action<GameObject, int> OnMonsterAttackPet;
    public event Action<GameObject, int> OnPlayerAttackEnemy;
    public event Action OnBattleStart;

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance(); 
        serviceLocator.SetEventManager(this);
    }


    public void TriggerPlayerBecameMainCharacter()
    {
        OnPlayerBecameMainCharacter?.Invoke();
    }

    public void TriggerPetBecameMainCharacter()
    {
        OnPetBecameMainCharacter.Invoke();
    }

    public void TriggerPetReturnedToBall(GameObject pet)
    {
        OnPetReturnedToBall?.Invoke(pet);
    }

    public void TriggerPetSummoned(GameObject pet)
    {
        OnPetSummoned?.Invoke(pet);
    }

    public void TriggerPetDeath()
    {
        OnPetDeath?.Invoke();
    }

    public void TriggerEnemyDeath()
    {
        OnEnemyDeath?.Invoke();
    }
    
    public void TriggerMonsterTouchPlayer()
    {
        OnMonsterTouchPlayer?.Invoke();
    }

    public void TriggerMonsterAttackPlayer(GameObject target, int amountOfDamage)
    {
        OnMonsterAttackPet?.Invoke(target, amountOfDamage);
    }

    public void TriggerPlayerAttackEnemy(GameObject target, int amountOfDamage)
    {
        OnPlayerAttackEnemy?.Invoke(target, amountOfDamage);
    }

    public void TriggerStartBattle()
    {
        OnBattleStart?.Invoke();
    }
}
