using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static event Action winBattle;
    public static event Action loseBattle;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    Transform targetLook;

    [SerializeField]
    private Transform petSlot1;

    private Transform currentPetSlot;

    [SerializeField] 
    private List<Transform> monsterSlots = new List<Transform>();

    private Transform currentMonsterSlot;

    private List<GameObject> allMonstersInBattle = new List<GameObject>();

    private List<Monster> monsters = new();
    private GameObject currentPet;


    [SerializeField]
    private MonsterDatabase monsterDatabase;
    [SerializeField]
    private BattleMonstersData mostersInBattle;
    private BattleMonstersData petsInBattle;

    private int currentNumberOfMonsters;

    [SerializeField]
    private GameObject playerGO;

    private GameObject targetGO;

    private CameraManager cameraManager;

    private static GameObject currentMainCharacter;
    public static GameObject CurrentMainCharacter => currentMainCharacter;

    private ServiceLocator serviceLocator;
    private EventManager eventManager;

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
        eventManager = serviceLocator.EventManager;
    }

    private void OnEnable()
    {
        if (eventManager == null) eventManager = serviceLocator.EventManager;
        eventManager.OnPetSummoned += SwapCameraTarget;
        //HealthManager.PetDied += ReturnCameraToPlayer;
    }

    private void OnDisable()
    {
        eventManager.OnPetSummoned -= SwapCameraTarget;
        //HealthManager.PetDied -= ReturnCameraToPlayer;
    }

    void Start()
    {
        cameraManager = GetComponent<CameraManager>();
        monsters = mostersInBattle.monsters;
        currentPetSlot = petSlot1;
        currentMonsterSlot = monsterSlots[0];
        AddPetsAndMonsters();
        SwapCameraTarget(currentPet);
        Debug.Log(currentMainCharacter.name);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnCameraToPlayer();
        }
    }

    private void AddPetsAndMonsters()
    {
        Vector3 directionToLook = targetLook.position - currentPetSlot.position;
        directionToLook.y = 0;
        Quaternion rotationToLook = Quaternion.LookRotation(directionToLook);
        targetGO = PetSpaceData.Pets[0].GOPet;
        currentPet = Instantiate(targetGO, currentPetSlot.position, rotationToLook);
        var healthManager = currentPet.GetComponent<HealthManager>();
        healthManager.SetMaxHealth(PetSpaceData.Pets[0].MaxHP);
        healthManager.SetCurrentHealth(PetSpaceData.Pets[0].CurrentHP);

        int i = 0;
        foreach (var monster in monsters) 
        {
            if (i > 4) return;
            directionToLook = targetLook.position - currentMonsterSlot.position;
            directionToLook.y = 0;
            rotationToLook = Quaternion.LookRotation(directionToLook);
            var go = Instantiate(monster.GOEnemy, currentMonsterSlot.position, rotationToLook);
            healthManager = go.GetComponent<HealthManager>();
            eventManager.OnEnemyDeath += CheckForLastMonsters;
            allMonstersInBattle.Add(go);
            if (i > 4) go.SetActive(false);
            ChooseMonsterSlot(i + 1);
        }
        currentNumberOfMonsters = allMonstersInBattle.Count;

    }

    private void CheckForLastMonsters()
    {
        currentNumberOfMonsters--;
        if (currentNumberOfMonsters == 0)
        {
            WinTheBattle();
        }
    }

    private void WinTheBattle()
    {
        winBattle?.Invoke();
        ReturnCameraToPlayer();
        foreach (var pet in PetSpaceData.Pets)
        {
            pet.AddExp(ExpForWin());
        }
        Invoke("LoadMainScene", 2f);
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    private int ExpForWin()
    {
        int exp = 0;
        foreach (var monster in allMonstersInBattle)
        {
            var baseHumanoid = monster.GetComponent<BaseHumanoid>();
            exp += baseHumanoid.Level * 5;
        }
        return exp;
    }

    private void SwapCameraTarget(GameObject target)
    {
        cameraManager.CameraFollowTarget(target);
        currentMainCharacter = target;
    }
    private void ReturnCameraToPlayer()
    {
        if (currentMainCharacter != player) Destroy(currentMainCharacter.gameObject);
        currentMainCharacter = player;
        cameraManager.CameraFollowPlayer();
    }

    private void ChooseMonsterSlot(int index)
    {
        currentMonsterSlot = monsterSlots[index];
    }
}
