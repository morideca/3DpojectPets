using UnityEngine;

public class MainCharacterManager : MonoBehaviour
{
    private ServiceLocator serviceLocator;

    private GameObject currentMainCharacter;

    private GameObject player;

    private CameraManager cameraManager;

    private EventManager eventManager;

    private void OnEnable()
    {
        if (eventManager == null) eventManager = serviceLocator.EventManager;
        eventManager.OnPetSummoned += MakePetMainCharacter;
        eventManager.OnPetDeath += MakePlayerMainCharacter;
    }

    private void OnDisable()
    {
        eventManager.OnPetSummoned -= MakePetMainCharacter;
        eventManager.OnPetDeath -= MakePlayerMainCharacter;
    }

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
    }

    private void Start()
    {
        eventManager = serviceLocator.EventManager;
        cameraManager = serviceLocator.CameraManager;
        player = serviceLocator.Player;
        currentMainCharacter = player;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MakePlayerMainCharacter();
        }
    }

    private void MakePetMainCharacter(GameObject pet)
    {
        currentMainCharacter = pet;
        serviceLocator.SetCurrentMainCharacter(pet);
        serviceLocator.SetPet(pet);
        eventManager.TriggerPetBecameMainCharacter();
    }

    private void MakePlayerMainCharacter()
    {
        if (currentMainCharacter != player)
        {
            eventManager.TriggerPetReturnedToBall(currentMainCharacter);
            DestroyPetGO();
            currentMainCharacter = player;
            serviceLocator.SetCurrentMainCharacter(player);
            eventManager.TriggerPlayerBecameMainCharacter();
            cameraManager.CameraFollowPlayer();
        }
    }

    private void DestroyPetGO()
    {
        Destroy(currentMainCharacter);
    }
}
