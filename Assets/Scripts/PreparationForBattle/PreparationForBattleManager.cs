using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreparationForBattleManager : MonoBehaviour
{
    [SerializeField]
    Transform targetLook;

    [SerializeField]
    private Transform petSlot1;
    [SerializeField]
    private Transform petSlot2;
    [SerializeField]
    private Transform petSlot3;
    [SerializeField]
    private Transform petSlot4;
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

    [SerializeField]
    private CinemachineFreeLook virtualCamera;
    private Transform cameraTarget;

    private int currentIndex = 0;

    void Start()
    {
        cameraManager = GetComponent<CameraManager>();

        monsters = mostersInBattle.monstersID;
        pets = petsInBattle.monstersID;

        currentPetSlot = petSlot1;
        currentMonsterSlot = monsterSlot1;
        AddPetsAndMonsters();

        cameraTarget = allMonstersGO[currentIndex].transform;

        cameraManager.SwapCameraTargetMain(allMonstersGO[0]);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeCameraTarget(true);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeCameraTarget(false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            allMonstersGO[currentIndex].GetComponent<Animator>().SetTrigger("cheer");
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartBattle();
        }
    }

    private void StartBattle()
    {
        SceneManager.LoadScene("Battle");
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
                        targetGO = monster.PreviewGO;
                    }
                }
                Vector3 directionToLook = targetLook.position - currentPetSlot.position;
                directionToLook.y = 0;
                Quaternion rotationToLook = Quaternion.LookRotation(directionToLook);
                var go = Instantiate(targetGO, currentPetSlot.position, rotationToLook);
                allMonstersGO.Add(go);
                SavePetGO(go);
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
                        targetGO = monster.PreviewGO;
                    }
                }
                Vector3 directionToLook = targetLook.position - currentMonsterSlot.position;
                directionToLook.y = 0;
                Quaternion rotationToLook = Quaternion.LookRotation(directionToLook);
                var go = Instantiate(targetGO, currentMonsterSlot.position, rotationToLook);
                allMonstersGO.Add(go);
                SaveMonsterGO(go);
                ChooseMonsterSlot(i + 1);
            }
        }
    }

    private void ChangeCameraTarget(bool left) 
    {
        int maxIndex = allMonstersGO.Count - 1;
        if (left)
        {
            currentIndex -= 1;
            if (currentIndex < 0) currentIndex = maxIndex;
        }
        else
        {
            currentIndex += 1;
            if (currentIndex > maxIndex) currentIndex = 0;
        }
        cameraManager.SwapCameraTargetMain(allMonstersGO[currentIndex]);
    }

    private void SaveMonsterGO(GameObject monster)
    {
        monstersGO.Add(monster);
        Debug.Log(monstersGO[0]);
    }

    private void SavePetGO(GameObject pet)
    {
        petsGO.Add(pet);
    }

    private void ChoosePetSlot(int index)
    {
        index += 1;
        switch(index)
        {
            case 0:
                currentPetSlot = petSlot1;
                break;
            case 1:
                currentPetSlot = petSlot2;
                break;
            case 2:
                currentPetSlot = petSlot3;
                break;
            case 3:
                currentPetSlot = petSlot4;
                break;
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
