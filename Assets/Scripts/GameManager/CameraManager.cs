using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum SceneType
{
    main,
    prepareForBattle,
    battle
}
public class CameraManager : MonoBehaviour
{
    public static event Action<GameObject> mainCharaterSwapped;
    public static event Action playerIsMainCharacter;
    public static event Action playerIsNotMainCharacter;

    [SerializeField]
    private CinemachineFreeLook virtualCamera;
    [SerializeField]
    private SceneType sceneType;

    public static SceneType SceneType;

    public static GameObject cameraTarget;

    private void Start()
    {
        SceneType = sceneType;
    }

    public void SwapCameraTargetMain(GameObject target)
    {

        switch (sceneType)
        {
            case SceneType.main:
                mainCharaterSwapped?.Invoke(target);
                playerIsNotMainCharacter?.Invoke();

                CameraManager.cameraTarget = target;

                var cameraTarget = target.transform.Find("CameraTarget");
                virtualCamera.Follow = cameraTarget;
                virtualCamera.LookAt = cameraTarget;
                
                break;
            case SceneType.prepareForBattle:
                break;
            case SceneType.battle:
                mainCharaterSwapped?.Invoke(target);

                CameraManager.cameraTarget = target;

                cameraTarget = target.transform.Find("CameraTarget");
                virtualCamera.Follow = cameraTarget;
                virtualCamera.LookAt = cameraTarget;
                break;
        }
    }

    public void ReturnCameraToPlayer(GameObject player, bool firstVoid)
    {
        if (!firstVoid) playerIsMainCharacter?.Invoke();
        cameraTarget = player;
        virtualCamera.Follow = player.transform.Find("CameraTarget");
        virtualCamera.LookAt = player.transform.Find("CameraTarget");
    }
}
