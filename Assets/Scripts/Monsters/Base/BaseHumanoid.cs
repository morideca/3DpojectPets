using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHumanoid : MonoBehaviour
{
    public static event Action<Monster> monsterJoinTheBattle;

    [SerializeField]
    private int id;
    private int level;
    [SerializeField]
    private int[] minMaxLevel;
    private Animator animator;
    private CharacterController controller;
    private HealthManager healthManager;
    private MonsterAI monsterAI;

    public Monster monster;

    public int ID => id;
    public int Level => level;

    public void IJoinTheBattleEvent()
    {
        monster.SetLevel(level);
        monsterJoinTheBattle?.Invoke(monster);
    }
    private void OnDisable()
    {
        healthManager.onDeathPrivate -= OnDeath;
        GameManager.battleStart -= JoinTheBattle;
    }

    private void Awake()
    {
        level = UnityEngine.Random.Range(minMaxLevel[0], minMaxLevel[1]);
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
        if (monsterAI.ISeePlayer)
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
