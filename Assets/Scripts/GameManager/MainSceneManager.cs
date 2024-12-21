using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField]
    private static SceneType sceneType;

    [SerializeField]
    private BattleMonstersData monstersForBattle;

    private ServiceLocator serviceLocator;
    private EventManager eventManager;

    void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
        serviceLocator.SetSceneType(sceneType);

    }

    private void OnEnable()
    {
        if (eventManager == null) eventManager = serviceLocator.EventManager;
        eventManager.OnMonsterTouchPlayer += BattleStarts;
        BaseHumanoid.monsterJoinTheBattle += AddMonsterToBattle;
    }

    private void OnDisable()
    {
        eventManager.OnMonsterTouchPlayer -= BattleStarts;
        BaseHumanoid.monsterJoinTheBattle -= AddMonsterToBattle;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void BattleStarts() 
    {
        monstersForBattle.monsters = new List<Monster>();
        eventManager.TriggerStartBattle();

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
