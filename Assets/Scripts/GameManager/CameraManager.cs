using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum SceneType
{
    main,
    prepareForBattle,
    battle
}
public class CameraManager : MonoBehaviour
{
    public static event Action<GameObject> mainCharacterSwapped;
    public static event Action playerIsMainCharacter;
    public static event Action playerIsNotMainCharacter;

    [SerializeField]
    private CinemachineFreeLook virtualCamera;

    [SerializeField]
    private  SceneType sceneType;

    public static SceneType SceneType;

    public static GameObject cameraTarget;

    private void Awake()
    {
        SceneType = sceneType;
    }

    public void SwapCameraTargetMain(GameObject target)
    {

        switch (SceneType)
        {
            case SceneType.main:

                CameraManager.cameraTarget = target;

                mainCharacterSwapped?.Invoke(target);
                playerIsNotMainCharacter?.Invoke();

                var cameraTarget = target.transform.Find("CameraTarget");
                if (cameraTarget == null) cameraTarget = target.transform;
                virtualCamera.Follow = cameraTarget;
                virtualCamera.LookAt = cameraTarget;
                break;

            case SceneType.prepareForBattle:

                CameraManager.cameraTarget = target;

                cameraTarget = target.transform.Find("CameraTarget");
                if (cameraTarget == null) cameraTarget = target.transform;
                virtualCamera.Follow = cameraTarget;
                virtualCamera.LookAt = cameraTarget;
                break;

            case SceneType.battle:
                mainCharacterSwapped?.Invoke(target);

                CameraManager.cameraTarget = target;
                cameraTarget = target.transform.Find("CameraTarget");
                if (cameraTarget == null) cameraTarget = target.transform;
                virtualCamera.Follow = cameraTarget;
                virtualCamera.LookAt = cameraTarget;
                break;
        }
    }

    public void ReturnCameraToPlayer(GameObject player, bool firstVoid)
    {
        if (!firstVoid) playerIsMainCharacter?.Invoke();
        mainCharacterSwapped?.Invoke(player);
        cameraTarget = player;
        virtualCamera.Follow = player.transform.Find("CameraTarget");
        virtualCamera.LookAt = player.transform.Find("CameraTarget");
    }
}
