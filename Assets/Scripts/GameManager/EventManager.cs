using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private ServiceLocator serviceLocator;

    public event Action OnPlayerBecameMainCharacter;
    public event Action OnPetBecameMainCharacter;
    public event Action<GameObject> OnPetReturnedToBall;
    public event Action<GameObject> OnPetSummoned;
    public event Action OnPetDeath;

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
}
