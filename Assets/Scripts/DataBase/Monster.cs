using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterTypes
{
    skeleton,
    skeletonMage,
    skeletonWarrior,
    golem   
}

[CreateAssetMenu(fileName = "Monster", menuName = "Game/Mosterdata")]
public class Monster : ScriptableObject
{
    [SerializeField]
    private string name;
    [SerializeField]
    private int id;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private string description;
    [SerializeField]
    private GameObject goEnemy;
    [SerializeField]
    private GameObject goPet;
    [SerializeField]
    private GameObject previewGO;
    [SerializeField]
    private MonsterTypes monsterType;



    public string Name => name;
    public int Id => id;
    public string Description => description;
    public GameObject GOEnemy => goEnemy;
    public GameObject GOPet => goPet;
    public GameObject PreviewGO => previewGO;
    public Sprite Icon => icon;
    public MonsterTypes MonsterType => monsterType;
}
