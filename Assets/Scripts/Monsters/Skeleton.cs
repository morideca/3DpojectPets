using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : BaseHumanoid
{


    private Animator animator;
    private CharacterController controller;
    private HealthManager healthManager;
    private MonsterAI monsterAI;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        healthManager.onDeathPrivate -= OnDeath;
        GameManager.battleStart -= JoinTheBattle;
    }

    private void Start()
    {
        monsterAI = GetComponent<MonsterAI>();
        animator = GetComponent<Animator>();
        animator.SetTrigger("spawned");
        controller = GetComponent<CharacterController>();
        healthManager = GetComponent<HealthManager>();
        healthManager.onDeathPrivate += OnDeath;
        GameManager.battleStart += JoinTheBattle;
    }
    private void OnDeath()
    {
        TryGetComponent<CapsuleCollider>(out var col);
        col.enabled = false;
        StartCoroutine(DestroyGameObjectCoroutine());
    }

    private void JoinTheBattle()
    {
        if (monsterAI.IsTaunted)
        {
            IJoinTheBattleEvent();
        }
    }

    private IEnumerator DestroyGameObjectCoroutine()
    {
        yield return new WaitForSecondsRealtime(30);
        Destroy(gameObject);
    }
}
