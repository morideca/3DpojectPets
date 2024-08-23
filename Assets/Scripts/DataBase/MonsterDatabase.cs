using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDataBase", menuName = "Game/MosterDataBase")]
public class MonsterDatabase : ScriptableObject
{
    [SerializeField]
    private List<Monster> monsters;

    public List<Monster> Monsters => monsters;
}
