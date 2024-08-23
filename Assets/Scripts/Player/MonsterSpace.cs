using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpace : MonoBehaviour
{
    private List<Monster> availableMonsters = new List<Monster>();

    public static event Action<Monster> setFirstSlot;
    public static event Action<Monster> setSecondSlot;
    public static event Action<Monster> setThirdSlot;
    public static event Action<Monster> setFourthSlot;
    private Monster slotOneID;
    private Monster slotTwoID;
    private Monster slotThreeID;
    private Monster slotFourID;

    public Monster SlotOneID => slotOneID;
    public Monster SlotTwoID => slotTwoID;
    public Monster SlotThreeID => slotThreeID;
    public Monster SlotFourID => slotFourID;


    private void OnEnable()
    {
        Ball.pickedUpTheBall += PickedUpMonster;
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
            if (monster.Id == monsterID)
            {
                availableMonsters.Add(monster);
                if (slotOneID == null)
                {
                    slotOneID = monster;
                    setFirstSlot(monster);
                }
                else if (slotTwoID == null)
                {
                    slotTwoID = monster;
                    setSecondSlot(monster);
                }
                else if (slotThreeID == null)
                {
                    slotThreeID = monster;
                    setThirdSlot(monster);
                }
                else if (slotFourID == null)
                {
                    slotFourID = monster;
                    setFourthSlot(monster);
                }
            }
        }
    }

    public List<int> CurrentPets()
    {
        List<int> pets = new List<int>();

        if (slotOneID != null) pets.Add(slotOneID.Id);
        if (slotTwoID != null) pets.Add(slotTwoID.Id);
        if (slotThreeID != null) pets.Add(slotThreeID.Id);
        if (slotFourID != null) pets.Add(slotFourID.Id);

        return pets;
    }

    public Monster monsterInCurrentSlot(int slotNumber)
    {
        switch (slotNumber)
        {
            case 1:
                return slotOneID;
            case 2:
                return slotTwoID;
            case 3:
                return slotThreeID;
            case 4:
                return slotFourID;
        }
        return null;
    }
}
