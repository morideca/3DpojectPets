using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpace : MonoBehaviour
{
    [SerializeField]
    private PetSpaceData petSpaceData;

    public static event Action<Pet, int> setPetSlot;

    private List<Pet> petInSlots;
    public List<Pet> PetInSlots => petInSlots;

    private int maxSlots = 4;

    public static MonsterSpace Instance;

    private ServiceLocator serviceLocator;
    private EventManager eventManager;


    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
        eventManager = serviceLocator.EventManager;
    }

    private void Start()
    {
        Instance = this;
        petInSlots = PetSpaceData.Pets;
    }

    private void OnEnable()
    {
        if (eventManager == null) eventManager = serviceLocator.EventManager;
        eventManager.OnPetReturnedToBall += SavePetStats;
    }

    private void OnDisable()
    {
        eventManager.OnPetReturnedToBall -= SavePetStats;
    }

    public void PickedUpMonster(int monsterID)
    {
        var monsters = Resources.Load<MonsterDatabase>("Database/MonsterDatabase").Monsters;
        foreach (var monster in monsters)
        {
            if (monster.Id == monsterID)
            {
                if (petInSlots == null || petInSlots.Count < maxSlots)
                {
                    var pet = new Pet(monster, 0, 1);
                    petInSlots.Add(pet);
                    setPetSlot?.Invoke(pet, petInSlots.Count);
                }
                else
                {

                }
            }
        }
    }

    private void SavePetStats(GameObject pet)
    {
        var _healthManager = pet.GetComponent<HealthManager>();
        int petIndex = PlayerThrowBall.CurrentMosnterSlot - 1;
        petInSlots[petIndex].SetCurrentHealth(_healthManager.CurrentHealth);
    }

    public Pet MonsterInCurrentSlot(int slotNumber)
    {
        return petInSlots[slotNumber - 1];
    }
}
