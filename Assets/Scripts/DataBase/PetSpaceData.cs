using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDataBase", menuName = "Game/PetDatabase")]


public class PetSpaceData : ScriptableObject
{
    private static List<Pet> pets = new List<Pet>();

    public static List<Pet> Pets => pets;

    public static void AddPet(Pet pet)
    {
        if (pets.Count < 4)
        {
            pets.Add(pet);
        }
        else Debug.Log("No enough place");
    }

    public static void AddPet(Pet pet, int slot)
    {
        if (slot > 3)
        {
            Debug.Log("There is no this slot");
            return;
        }
        pets[slot - 1] = pet;
    }
}
