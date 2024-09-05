using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Pet 
{
    private string name;
    private int id;
    private int currentExp;
    private int maxExp;
    private int currentLevel;
    private int currentHP;
    private int maxHP;
    private Sprite icon;
    private string description;
    private GameObject goEnemy;
    private GameObject goPet;
    private GameObject previewGO;
    private MonsterTypes monsterType;

    private bool canEvolve;

    public bool CanEvolve => canEvolve;

    public string Name => name;
    public int Id => id;
    public string Description => description;
    public GameObject GOPet => goPet;
    public GameObject PreviewGO => previewGO;
    public Sprite Icon => icon;
    public MonsterTypes MonsterType => monsterType;
    public int CurrentExp => currentExp;
    public int CurrentLevel => currentLevel;
    public int CurrentHP => currentHP;
    public int MaxExp => maxExp;
    public int MaxHP => maxHP;

    public Pet(Monster monster, int currentExp, int currentLevel)
    {
        this.name = monster.Name;
        this.id = monster.Id;
        this.icon = monster.Icon;
        this.description = monster.Description;
        this.goPet = monster.GOPet;
        this.previewGO = monster.PreviewGO;
        this.monsterType = monster.MonsterType;
        this.currentExp = currentExp;
        this.currentLevel = currentLevel;
        CountMaxExp();
        CountMaxHealth();
        currentHP = maxHP;
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= maxExp)
        {
            LevelUp();
            int exp = currentExp - maxExp;
            currentExp = exp;
        }
    }

    public void CountMaxExp()
    {
        maxExp = currentLevel * 10;
    }

    public void CountMaxHealth()
    {
        maxHP = 100 + currentLevel * 20;
    }

    public void SetCurrentHealth(int amount)
    {
        currentHP = amount;
    }

    public void LevelUp()
    {
        currentLevel++;
        CountMaxExp();
        CountMaxHealth();
    }
}
