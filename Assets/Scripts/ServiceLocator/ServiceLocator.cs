using UnityEngine;

public class ServiceLocator
{
    public GameObject Player {  get; private set; }

    public GameObject Pet { get; private set; }

    public CameraManager CameraManager { get; private set; }

    public GameObject CurrentMainCharacter { get; private set; }

    public EventManager EventManager { get; private set; }

    private static ServiceLocator instance; 

    public static ServiceLocator GetInstance()
    {
        if (instance == null) instance = new ServiceLocator();
        return instance;
    }

    public void SetPet(GameObject pet)
    {
        Pet = pet;
    }

    public void SetCurrentMainCharacter(GameObject mainCharacter)
    {
        CurrentMainCharacter = mainCharacter;
    }

    public void SetPlayer(GameObject player)
    {
        this.Player = player;
    }

    public void SetCameraManager(CameraManager cameraManager)
    {
        this.CameraManager = cameraManager;
    }

    public void SetEventManager(EventManager eventManager)
    {
        this.EventManager = eventManager;
    }
}
