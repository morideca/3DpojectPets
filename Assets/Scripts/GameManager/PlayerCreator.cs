using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject playerGO;

    private GameObject player;

    [SerializeField]
    private Transform playerSpawn;

    private ServiceLocator serviceLocator;

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
    }
    void Start()
    {
        player = Instantiate(playerGO, playerSpawn.position, Quaternion.identity);
        serviceLocator.SetPlayer(player);
        serviceLocator.SetCurrentMainCharacter(player);
    }

    void Update()
    {
        
    }
}
