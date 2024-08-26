using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHumanoid : MonoBehaviour
{
    public static event Action<int> monsterJoinTheBattle;

    [SerializeField]
    private int id;
    private Animator animator;
    private CharacterController controller;
    private HealthManager healthManager;
    private MonsterAI monsterAI;

    public int ID => id;

    public void IJoinTheBattleEvent()
    {
        monsterJoinTheBattle?.Invoke(id);
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
