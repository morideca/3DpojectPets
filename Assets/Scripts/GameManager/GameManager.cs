using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private static SceneType sceneType;
    [SerializeField]
    private GameObject playerGO;
    [SerializeField]
    private GameObject skeletonGO;
    [SerializeField]
    private Transform playerSwapn;
    [SerializeField]
    private Transform skeletonSwapn;
    [SerializeField]
    private BattleMonstersData monstersForBattle;
    [SerializeField]
    private BattleMonstersData petsForBattle;


    public static event Action battleStart;

    private static GameObject player;
    private static GameObject currentMainCharacter;
    private static GameObject currentCameraTarget;

    public static GameObject CurrentMainCharacter => currentMainCharacter;
    public static GameObject Player => player;
    public static GameObject CurrentCameraTarget => currentCameraTarget;
    public static SceneType SceneType => sceneType;

    private CameraManager cameraManager;
    private void OnEnable()
    {
        BallWithMonster.petSummoned += SwapCameraTarget;
        MonsterAI.monsterToutchedPlayer += BattleStarts;
        HealthManager.petDied += ReturnCameraToPlayer;
        BaseHumanoid.monsterJoinTheBattle += AddMonsterToBattle;
    }

    private void OnDisable()
    {
        BallWithMonster.petSummoned -= SwapCameraTarget;
        MonsterAI.monsterToutchedPlayer -= BattleStarts;
        HealthManager.petDied -= ReturnCameraToPlayer;
        BaseHumanoid.monsterJoinTheBattle -= AddMonsterToBattle;
    }

    void Awake()
    {
        cameraManager = GetComponent<CameraManager>();

        player = Instantiate(playerGO, playerSwapn.position, Quaternion.identity);
        currentMainCharacter = player;

        Instantiate(skeletonGO, skeletonSwapn.position, Quaternion.identity);

        cameraManager.ReturnCameraToPlayer(player, true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnCameraToPlayer();
        }
    }

    private void SwapCameraTarget(GameObject target)
    {
        cameraManager.SwapCameraTargetMain(target);
        currentMainCharacter = target;
    }

    private void ReturnCameraToPlayer()
    {
        currentMainCharacter = player;
        cameraManager.ReturnCameraToPlayer(player, false);
    }

    private void BattleStarts() 
    {
        monstersForBattle.monstersID = new List<int>();
        battleStart?.Invoke();
        petsForBattle.monstersID = player.GetComponent<MonsterSpace>().CurrentPets();

        LoadPreparationScence();
    }

    private void AddMonsterToBattle(int monsterID)
    {
        monstersForBattle.monstersID.Add(monsterID);
    }

    private void LoadPreparationScence()
    {
        SceneManager.LoadScene("PrepareForBattle");
    }
}
