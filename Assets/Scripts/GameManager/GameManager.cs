using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<GameObject> OnPetReturn;

    [SerializeField]
    private static SceneType sceneType;
    [SerializeField]
    private GameObject playerGO;
    [SerializeField]
    private Transform playerSwapn;
    [SerializeField]
    private BattleMonstersData monstersForBattle;
    [SerializeField]
    private BattlePetData petsForBattle;


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
        BallWithMonster.PetSummoned += SwapCameraTarget;
        MonsterAI.monsterToutchedPlayer += BattleStarts;
        HealthManager.petDied += ReturnCameraToPlayer;
        BaseHumanoid.monsterJoinTheBattle += AddMonsterToBattle;
    }

    private void OnDisable()
    {
        BallWithMonster.PetSummoned -= SwapCameraTarget;
        MonsterAI.monsterToutchedPlayer -= BattleStarts;
        HealthManager.petDied -= ReturnCameraToPlayer;
        BaseHumanoid.monsterJoinTheBattle -= AddMonsterToBattle;
    }

    void Awake()
    {
        cameraManager = GetComponent<CameraManager>();

        player = Instantiate(playerGO, playerSwapn.position, Quaternion.identity);
        currentMainCharacter = player;

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
        if (currentMainCharacter != player)
        {
            OnPetReturn?.Invoke(currentMainCharacter);
            Destroy(currentMainCharacter.gameObject);
        }
        currentMainCharacter = player;
        cameraManager.ReturnCameraToPlayer(player, false);
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
