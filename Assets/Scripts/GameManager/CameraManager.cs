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
    public static event Action<GameObject> OnMainCharacterSwapped;
    public static event Action OnPlayerIsMainCharacter;
    public static event Action OnPlayerIsNotMainCharacter;

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

                OnMainCharacterSwapped?.Invoke(target);
                OnPlayerIsNotMainCharacter?.Invoke();

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
                OnMainCharacterSwapped?.Invoke(target);
                OnPlayerIsNotMainCharacter?.Invoke();

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
        if (!firstVoid) OnPlayerIsMainCharacter?.Invoke();
        OnMainCharacterSwapped?.Invoke(player);
        cameraTarget = player;
        virtualCamera.Follow = player.transform.Find("CameraTarget");
        virtualCamera.LookAt = player.transform.Find("CameraTarget");
    }
}
