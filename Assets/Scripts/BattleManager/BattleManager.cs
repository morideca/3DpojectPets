using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    
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

    private List<GameObject> monstersGO = new List<GameObject>();
    private List<GameObject> petsGO = new List<GameObject>();

    private List<GameObject> allMonstersGO = new List<GameObject>();

    private List<int> monsters = new List<int>();
    private List<int> pets = new List<int>();

    [SerializeField]
    private MonsterDatabase monsterDatabase;
    [SerializeField]
    private BattleMonstersData mostersInBattle;
    [SerializeField]
    private BattleMonstersData petsInBattle;

    private GameObject targetGO;

    private CameraManager cameraManager;

    private Transform cameraTarget;

    private static GameObject currentMainCharacter;
    public static GameObject CurrentMainCharacter => currentMainCharacter;

    private int currentIndex = 0;

    void Start()
    {
        cameraManager = GetComponent<CameraManager>();
        monsters = mostersInBattle.monstersID;
        pets = petsInBattle.monstersID;
        currentPetSlot = petSlot1;
        currentMonsterSlot = monsterSlot1;
        AddPetsAndMonsters();

        cameraManager.SwapCameraTargetMain(allMonstersGO[0]);
    }

    private void Update()
    {

    }

    private void AddPetsAndMonsters()
    {
        for (int i = 0; i < pets.Count; i++)
        {
            if (pets[i] > 0)
            {
                foreach (var monster in monsterDatabase.Monsters)
                {
                    if (monsters[i] == monster.Id)
                    {
                        targetGO = monster.GOPet;
                    }
                }
                Vector3 directionToLook = targetLook.position - currentPetSlot.position;
                directionToLook.y = 0;
                Quaternion rotationToLook = Quaternion.LookRotation(directionToLook);
                var go = Instantiate(targetGO, currentPetSlot.position, rotationToLook);
                allMonstersGO.Add(go);
                if (i > 0) go.SetActive(false);
                ChooseMonsterSlot(i);
            }
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] > 0)
            {
                foreach (var monster in monsterDatabase.Monsters)
                {
                    if (monsters[i] == monster.Id)
                    {
                        targetGO = monster.GOEnemy;
                    }
                }
                Vector3 directionToLook = targetLook.position - currentMonsterSlot.position;
                directionToLook.y = 0;
                Quaternion rotationToLook = Quaternion.LookRotation(directionToLook);
                var go = Instantiate(targetGO, currentMonsterSlot.position, rotationToLook);
                allMonstersGO.Add(go);
                if (i > 4) go.SetActive(false);
                ChooseMonsterSlot(i + 1);
            }
        }
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
