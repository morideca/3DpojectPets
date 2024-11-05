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
    private Transform monsterSlot1;
    [SerializeField]
    private Transform monsterSlot2;
    [SerializeField]
    private Transform monsterSlot3;
    [SerializeField]
    private Transform monsterSlot4;
    [SerializeField]
    private Transform monsterSlot5;
    private Transform currentMonsterSlot;

    private List<GameObject> allMonstersInBattle = new List<GameObject>();

    private List<Monster> monsters = new();
    private GameObject currentPet;


    [SerializeField]
    private MonsterDatabase monsterDatabase;
    [SerializeField]
    private BattleMonstersData mostersInBattle;
    [SerializeField]
    private BattleMonstersData petsInBattle;

    private int currentNumberOfMonsters;

    [SerializeField]
    private GameObject playerGO;

    private GameObject targetGO;

    private CameraManager cameraManager;

    private static GameObject currentMainCharacter;
    public static GameObject CurrentMainCharacter => currentMainCharacter;

    private void OnEnable()
    {
        BallWithMonster.PetSummoned += SwapCameraTarget;
        HealthManager.petDied += ReturnCameraToPlayer;
    }

    private void OnDisable()
    {
        BallWithMonster.PetSummoned -= SwapCameraTarget;
        HealthManager.petDied -= ReturnCameraToPlayer;
    }

    void Start()
    {
        cameraManager = GetComponent<CameraManager>();
        monsters = mostersInBattle.monsters;
        currentPetSlot = petSlot1;
        currentMonsterSlot = monsterSlot1;
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
            healthManager.onDeathPrivate += CheckForLastMonsters;
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
        cameraManager.SwapCameraTargetMain(target);
        currentMainCharacter = target;
    }
    private void ReturnCameraToPlayer()
    {
        if (currentMainCharacter != player) Destroy(currentMainCharacter.gameObject);
        currentMainCharacter = player;
        cameraManager.ReturnCameraToPlayer(player, true);
    }

    private void ChooseMonsterSlot(int index)
    {

        switch (index)
        {
            case 0:
                currentMonsterSlot = monsterSlot1;
                break;
            case 1:
                currentMonsterSlot = monsterSlot2;
                break;
            case 2:
                currentMonsterSlot = monsterSlot3;
                break;
            case 3:
                currentMonsterSlot = monsterSlot4;
                break;
            case 4:
                currentMonsterSlot = monsterSlot5;
                break;
        }
    }
}
