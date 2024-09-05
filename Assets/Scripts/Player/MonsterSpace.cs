using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpace : MonoBehaviour
{
    //[SerializeField]
    //private BattleMonstersData petsInSpaceData;
    [SerializeField]
    private PetSpaceData petSpaceData;

    public static event Action<Pet> setFirstSlot;
    public static event Action<Pet> setSecondSlot;
    public static event Action<Pet> setThirdSlot;
    public static event Action<Pet> setFourthSlot;

    private List<Pet> petInSlots;
    public List<Pet> PetInSlots => petInSlots;

    private int maxSlots = 4;

    private void Start()
    {
        GameManager.OnPetReturn += SavePetStats;
        Ball.pickedUpTheBall += PickedUpMonster;
        petInSlots = PetSpaceData.Pets;
    }

    private void OnEnable()
    {
 
    }

    private void OnDisable()
    {
        Ball.pickedUpTheBall -= PickedUpMonster;
        GameManager.OnPetReturn -= SavePetStats;
    }

    private void PickedUpMonster(int monsterID)
    {
        var monsters = Resources.Load<MonsterDatabase>("Database/MonsterDatabase").Monsters;
        foreach (var monster in monsters)
        {
            if (monster.Id == monsterID)
            {
                if (petInSlots == null || petInSlots.Count < maxSlots)
                {
                    Debug.Log("add");
                    var pet = new Pet(monster, 0, 1);
                    petInSlots.Add(pet);
                    if (petInSlots.Count == 1) setFirstSlot?.Invoke(pet);
                    else if (petInSlots.Count == 2) setSecondSlot?.Invoke(pet);
                    else if (petInSlots.Count == 3) setThirdSlot?.Invoke(pet);
                    else if (petInSlots.Count == 4) setFourthSlot?.Invoke(pet);
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
        switch (slotNumber)
        {
            case 1:
                return petInSlots[0];
            case 2:
                return petInSlots[1];
            case 3:
                return petInSlots[2];
            case 4:
                return petInSlots[3];
        }
        return null;
    }


}
