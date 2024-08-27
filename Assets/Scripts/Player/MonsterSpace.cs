using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;

public class MonsterSpace : MonoBehaviour
{
    [SerializeField]
    private BattleMonstersData petsInSpaceData;

    public static event Action<Monster> setFirstSlot;
    public static event Action<Monster> setSecondSlot;
    public static event Action<Monster> setThirdSlot;
    public static event Action<Monster> setFourthSlot;

    private List<Monster> monsterSlots = new();
    public List<Monster> MonsterSlots => monsterSlots;

    private int maxSlots = 4;

    private void Start()
    {
        Ball.pickedUpTheBall += PickedUpMonster;
        LoadCurrentPets();
    }

    private void OnEnable()
    {
 
    }

    private void OnDisable()
    {
        Ball.pickedUpTheBall -= PickedUpMonster;
    }

    private void PickedUpMonster(int monsterID)
    {
        var monsters = Resources.Load<MonsterDatabase>("Database/MonsterDatabase").Monsters;

        foreach (var monster in monsters)
        {
            Debug.Log("monsterId:" + monster.Id);
            Debug.Log("monster" + monsterID);
            if (monster.Id == monsterID)
            {
                if (monsterSlots.Count < maxSlots)
                {
                    Debug.Log("add");
                    monsterSlots.Add(monster);

                    if (monsterSlots.Count == 1) setFirstSlot?.Invoke(monster);
                    else if (monsterSlots.Count == 2) setSecondSlot?.Invoke(monster);
                    else if (monsterSlots.Count == 3) setThirdSlot?.Invoke(monster);
                    else if (monsterSlots.Count == 4) setFourthSlot?.Invoke(monster);
                }
                else
                {

                }
            }
        }
    }

    public List<int> CurrentPets()
    {
        List<int> pets = new List<int>();


        for (int i = 0; i < monsterSlots.Count; i++)
        {
            if (monsterSlots[i] != null)
            {
                pets.Add(monsterSlots[i].Id);

            }
        }
        return pets;
    }

    private void LoadCurrentPets()
    {
        var petData = Resources.Load<MonsterDatabase>("Database/MonsterDatabase").Monsters;
        Debug.Log(monsterSlots.Count);

        foreach (var pet in petsInSpaceData.monstersID)
        {
            foreach (var monster in petData)

                if (monster.Id == pet)
                {
                    monsterSlots.Add(monster);

                    if (monsterSlots.Count == 1)
                    {
                        setFirstSlot?.Invoke(monster);
                    }
                    else if (monsterSlots.Count == 2) setSecondSlot?.Invoke(monster);
                    else if (monsterSlots.Count == 3) setThirdSlot?.Invoke(monster);
                    else if (monsterSlots.Count == 4) setFourthSlot?.Invoke(monster);
                }
        }
    }

    public Monster MonsterInCurrentSlot(int slotNumber)
    {
        switch (slotNumber)
        {
            case 1:
                return monsterSlots[0];
            case 2:
                return monsterSlots[1];
            case 3:
                return monsterSlots[2];
            case 4:
                return monsterSlots[3];
        }
        return null;
    }


}
