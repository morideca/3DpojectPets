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
    protected string name;
    [SerializeField]
    protected int id;
    [SerializeField]
    protected Sprite icon;
    [SerializeField]
    protected string description;
    [SerializeField]
    protected GameObject goEnemy;
    [SerializeField]
    protected GameObject goPet;
    [SerializeField]
    protected GameObject previewGO;
    [SerializeField]
    protected MonsterTypes monsterType;
    private int level;

    [SerializeField]
    protected Monster evolutionForm;

    public string Name => name;
    public int Id => id;
    public string Description => description;
    public GameObject GOEnemy => goEnemy;
    public GameObject GOPet => goPet;
    public GameObject PreviewGO => previewGO;
    public Sprite Icon => icon;
    public MonsterTypes MonsterType => monsterType;
    public int Level => level;
    public Monster EvolutionForm => evolutionForm;

    public void SetLevel(int amount)
    {
        level = amount;
    }
}
