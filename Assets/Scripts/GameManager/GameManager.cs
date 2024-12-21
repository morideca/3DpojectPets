using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private static SceneType sceneType;

    [SerializeField]
    private BattleMonstersData monstersForBattle;

    public static event Action battleStart;

    public SceneType SceneType => sceneType;

    private ServiceLocator serviceLocator;

    private void OnEnable()
    {
        MonsterAI.monsterToutchedPlayer += BattleStarts;
 
        BaseHumanoid.monsterJoinTheBattle += AddMonsterToBattle;
    }

    private void OnDisable()
    {
        MonsterAI.monsterToutchedPlayer -= BattleStarts;
        BaseHumanoid.monsterJoinTheBattle -= AddMonsterToBattle;
    }

    void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();

    }
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void BattleStarts() 
    {
        monstersForBattle.monsters = new List<Monster>();
        battleStart?.Invoke();

        LoadPreparationScence();
    }

    private void AddMonsterToBattle(Monster monster)
    {
        monstersForBattle.monsters.Add(monster);
    }

    private void LoadPreparationScence()
    {
        SceneManager.LoadScene("PrepareForBattle");
    }
}
