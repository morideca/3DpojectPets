using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHumanoid : MonoBehaviour
{
    public static event Action<int> monsterJoinTheBattle;

    [SerializeField]
    private int id;

    public int ID => id;

    public void IJoinTheBattleEvent()
    {
        monsterJoinTheBattle?.Invoke(id);
    }
}
