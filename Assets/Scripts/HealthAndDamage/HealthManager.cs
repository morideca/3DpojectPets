using System;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDamageable
{
    public event Action<int> OnGetDamage;

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
        OnGetDamage?.Invoke(currentHealth);
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
            else eventManager.TriggerEnemyDeath();
            animator.SetTrigger("die");

        }
        isDead = true;
    }

    public void GetDamage(int damage)
    {
        animator.SetTrigger("hitted");
        currentHealth -= damage;
        OnGetDamage?.Invoke(currentHealth);
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
