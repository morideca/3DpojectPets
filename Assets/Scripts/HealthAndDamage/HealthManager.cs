using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour, IDamageable
{
    public event Action<int> wasDamaged;
    public static event Action petDied;

    public static event Action onDeath;
    public event Action onDeathPrivate;
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

    void Start()
    {
        animator = GetComponent<Animator>();
        if (GameManager.CurrentMainCharacter != null && GameManager.CurrentMainCharacter == this.gameObject) IAmPet = true;
        else if (BattleManager.CurrentMainCharacter != null && BattleManager.CurrentMainCharacter == this.gameObject) IAmPet = true;

        if (!IAmPet)
        {
            var baseHumanoid = GetComponent<BaseHumanoid>();
            maxHealth = 100 + baseHumanoid.Level * healthLevelScale;
            currentHealth = maxHealth;
            Debug.Log("my level is: " + baseHumanoid.Level);
        }
        Debug.Log("my maxHealth is: " + maxHealth);
        wasDamaged?.Invoke(currentHealth);
    }

    void Update()
    {

    }

    public void Death()
    {
        if (isDead == false)
        {
            if (IAmPet) petDied?.Invoke();
            animator.SetTrigger("die");
            onDeath?.Invoke();
            onDeathPrivate?.Invoke();
        }
        isDead = true;
    }

    public void GetDamage(int damage)
    {
        animator.SetTrigger("hitted");
        currentHealth -= damage;
        wasDamaged?.Invoke(currentHealth);
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

    public void GetHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }


}
