using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private int startMonsterCount;

    [SerializeField]
    private MonsterTypes monsterType;
    [SerializeField]
    private Vector2 spawnAreaSize;
    [SerializeField]
    private Transform swapnCenterPoint;
    [SerializeField]
    private LayerMask groundLayer;

    private float spawnCooldown = 5;

    private Vector3 spawnPoint;

    private GameObject monsterGO;

    

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        MonsterDatabase monsterData = Resources.Load<MonsterDatabase>("Database/MonsterDatabase");
        foreach (var monster in monsterData.Monsters)
        {
            if (monster.MonsterType == monsterType) monsterGO = monster.GOEnemy;
        }
        SpawnMonster(startMonsterCount);
    }

    private Vector3 RandomPosition()
    {
        var randomPosition = new Vector3(Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2), 0,
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2));
        randomPosition += swapnCenterPoint.position;
        return randomPosition;
    }

    private void SetSpawnPoint(Vector3 transform)
    {
        Physics.Raycast(transform, Vector3.down, out var hit, 100, groundLayer);
        spawnPoint = hit.point;
    }

    private void SpawnMonster(int count)
    {
        for (int i = 0; i < count; i++)
        {
            InstantiateMonster();
        }
    }

    private void InstantiateMonster()
    {
        SetSpawnPoint(RandomPosition());
        var monster = Instantiate(monsterGO, spawnPoint, Quaternion.identity);
        monster.GetComponent<HealthManager>().onDeathPrivate += StartSpawnCooldown;
    }

    private void StartSpawnCooldown()
    {
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(spawnCooldown);
        InstantiateMonster();
    }
}
