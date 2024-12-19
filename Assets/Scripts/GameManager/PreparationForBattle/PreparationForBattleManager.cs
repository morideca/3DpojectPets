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
    private List<Transform> petSlots = new List<Transform>();
    [SerializeField]
    private List<Transform> monsterSlots = new List<Transform>();

    private Transform currentPetSlot;

    private Transform currentMonsterSlot;


    private List<GameObject> monstersGO = new List<GameObject>();
    private List<GameObject> petsGO = new List<GameObject>();
    private List<GameObject> allMonstersGO = new List<GameObject>();

    private List<Monster> monsters = new();
    private List<Pet> pets = new();

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

        monsters = mostersInBattle.monsters;
        pets = PetSpaceData.Pets;

        currentPetSlot = petSlots[0];
        currentMonsterSlot = monsterSlots[0];
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
        int i = 0;
        foreach (var pet in pets)
        {
            Vector3 directionToLook = targetLook.position - currentPetSlot.position;
            directionToLook.y = 0;
            Quaternion rotationToLook = Quaternion.LookRotation(directionToLook);
            var go = Instantiate(pet.PreviewGO, currentPetSlot.position, rotationToLook);
            allMonstersGO.Add(go);
            SavePetGO(go);
            ChoosePetSlot(i);
            i++;
        }

        i = 0;
        foreach (var monster in monsters)
        {
            Vector3 directionToLook = targetLook.position - currentMonsterSlot.position;
            directionToLook.y = 0;
            Quaternion rotationToLook = Quaternion.LookRotation(directionToLook);
            var go = Instantiate(monster.PreviewGO, currentMonsterSlot.position, rotationToLook);
            allMonstersGO.Add(go);
            SaveMonsterGO(go);
            i++;
            ChooseMonsterSlot(i);
 
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
        currentPetSlot = petSlots[index];
    }

    private void ChooseMonsterSlot(int index)
    {
        currentMonsterSlot = monsterSlots[index];
    }
}
