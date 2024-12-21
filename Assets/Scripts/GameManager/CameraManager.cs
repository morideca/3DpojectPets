using Cinemachine;
using System;
using Unity.VisualScripting;
using UnityEngine;
public class CameraManager : MonoBehaviour
{
    private ServiceLocator serviceLocator;
    private EventManager eventManager;

    [SerializeField]
    private CinemachineFreeLook virtualCamera;

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
        serviceLocator.SetCameraManager(this);
        eventManager = serviceLocator.EventManager;
    }

    private void Start()
    {
        CameraFollowTarget(serviceLocator.CurrentMainCharacter);
    }

    private void OnEnable()
    {
        if (eventManager == null) eventManager = serviceLocator.EventManager;
        eventManager.OnPetSummoned += CameraFollowTarget;
    }

    private void OnDisable()
    {
        eventManager.OnPetSummoned -= CameraFollowTarget;
    }

    public void CameraFollowTarget(GameObject target)
    {
        var cameraTarget = CameraPoint(target);
        if (cameraTarget == null) cameraTarget = target.transform;
        CameraLookAt(cameraTarget);
    }

    public void CameraFollowPlayer()
    {
        var player = serviceLocator.Player;
        var cameraTarget = CameraPoint(player);
        CameraLookAt(cameraTarget);
    }

    private void CameraLookAt(Transform cameraTarget)
    {
        virtualCamera.Follow = cameraTarget;
        virtualCamera.LookAt = cameraTarget;
    }

    private Transform CameraPoint(GameObject target)
    {
        var cameraPoint = target.transform.Find("CameraTarget");
        if (cameraPoint == null)
        {
            Debug.Log("�� ������� ��� CameraPoint!");
        }
        return cameraPoint;
    }
}
