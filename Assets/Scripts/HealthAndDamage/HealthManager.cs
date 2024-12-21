using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour, IDamageable
{
    public event Action<int> WasDamaged;
    public static event Action onDeath;
    public event Action OnDeathPrivate;

    [SerializeField]
    private int healthLevelScale;
    [SerializeField]
    private int maxHealth;
    private int currentHealth;

    private bool isDead = false;
    private bool IAmPet = false;

    public bool IsDead => isDead;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    private Animator animator;

    private Pet pet;

    private ServiceLocator serviceLocator;
    private EventManager eventManager;

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
    }

    void Start()
    {
        eventManager = serviceLocator.EventManager;
        animator = GetComponent<Animator>();
        if (serviceLocator.CurrentMainCharacter != null && serviceLocator.CurrentMainCharacter == this.gameObject) IAmPet = true;
        else if (BattleManager.CurrentMainCharacter != null && BattleManager.CurrentMainCharacter == this.gameObject) IAmPet = true;
        if (!IAmPet)
        {
            var baseHumanoid = GetComponent<BaseHumanoid>();
            maxHealth = 100 + baseHumanoid.Level * healthLevelScale;
            currentHealth = maxHealth;
            Debug.Log("my level is: " + baseHumanoid.Level);
        }
        Debug.Log("my maxHealth is: " + maxHealth);
        WasDamaged?.Invoke(currentHealth);
    }

    private void Init()
    {
        SetCurrentHealth(pet.CurrentHP);
        SetMaxHealth(pet.MaxHP);
    }

    public void Death()
    {
        if (isDead == false)
        {
            if (IAmPet) eventManager.TriggerPetDeath();
            animator.SetTrigger("die");
            onDeath?.Invoke();
            OnDeathPrivate?.Invoke();
        }
        isDead = true;
    }

    public void GetDamage(int damage)
    {
        animator.SetTrigger("hitted");
        currentHealth -= damage;
        WasDamaged?.Invoke(currentHealth);
        if (currentHealth <= 0) Death();
    }

    public void SetCurrentHealth(int value)
    {
        currentHealth = value;
    }

    public void SetMaxHealth(int value)
    {
        maxHealth = value;
    }

    public void SetPetClass(Pet pet)
    {
        this.pet = pet;
        Init();
    }

    public void GetHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }


}
