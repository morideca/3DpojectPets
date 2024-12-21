using Cinemachine;
using System;
using UnityEngine;

public enum TypeOfBall
{
    simpleBall,
    bronzeBall,
    silverBall,
    goldenBall,
    ballWithMonster
}

public class PlayerThrowBall : MonoBehaviour
{
    public static event Action<int> setCurrentPetSlot;

    private Animator animator;
    private LineRenderer lineRenderer;
    private MonsterSpace monsterSpace;
    private PlayerHumanoidMove mainCharacterMove;

    private ServiceLocator serviceLocator;
    private EventManager eventManager;

    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject ballWithMonster;
    [SerializeField]
    private float ballForce;
    [SerializeField]
    private Transform pointForLaunch;
    [SerializeField]
    private CinemachineFreeLook virtualCamera;
    private GameObject mainCamera;

    private bool canThrow = true;

    private static int currentMonsterSlot = 1;
    public static int CurrentMosnterSlot => currentMonsterSlot;

    private Vector3 ballTrajectory;
    private Vector3 ballTrajectoryAdd = new Vector3(0, 0.5f, 0);

    private TypeOfBall typeOfBall;

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
        eventManager = serviceLocator.EventManager;
    }
    private void OnEnable()
    {
        if (eventManager == null) eventManager = serviceLocator.EventManager;
        eventManager.OnPlayerBecameMainCharacter += AllowThrow;
        eventManager.OnPetBecameMainCharacter += BanThrow;
    }

    private void OnDisable()
    {
        eventManager.OnPlayerBecameMainCharacter -= AllowThrow;
        eventManager.OnPetBecameMainCharacter -= BanThrow;
    }

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        animator = GetComponent<Animator>();
        mainCharacterMove = GetComponent<PlayerHumanoidMove>();
        monsterSpace = GetComponent<MonsterSpace>();
    }

    private void AllowThrow() => canThrow = true;

    private void BanThrow() => canThrow = false;

    private void ThrowBall()
    {
        switch(typeOfBall)
        {
            case TypeOfBall.simpleBall:
                var newBall = Instantiate(ball, pointForLaunch.position, Quaternion.identity);
                newBall.GetComponent<Rigidbody>().velocity = ballTrajectory * ballForce;
                break;
            case TypeOfBall.ballWithMonster:
                Pet pet = monsterSpace.MonsterInCurrentSlot(currentMonsterSlot);
                newBall = Instantiate(ballWithMonster, pointForLaunch.position, Quaternion.identity);
                newBall.GetComponent<BallWithMonster>().PutMosnterIn(pet);
                newBall.GetComponent<Rigidbody>().velocity = ballTrajectory * ballForce;
                break;
        }
    }

    private void ShowTrajectory()
    {
        ballForce = Mathf.Clamp(ballForce, 7, 15);

        Vector3[] points = new Vector3[25];

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            ballTrajectory = mainCamera.transform.forward;
            ballTrajectory += ballTrajectoryAdd;
            ballTrajectory = ballTrajectory.normalized;
            points[i] = pointForLaunch.position + ballTrajectory * ballForce * time + Physics.gravity * time * time / 2f;
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    private void ThrowBallUpdate()
    {
        if (canThrow)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                lineRenderer.enabled = true;
                mainCharacterMove.FreezeRotation();
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                ShowTrajectory();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                animator.SetTrigger("throw");
                lineRenderer.enabled = false;
            }
        }
    }

    private void ChooseSimpleBallUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            setCurrentPetSlot?.Invoke(0);
            typeOfBall = TypeOfBall.simpleBall;
            canThrow = true;
        }
    }

    private void ChoosePetSlotUpdate()
    {
        int i = 0;
        if (monsterSpace.PetInSlots.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                i = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                i = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                i = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                i = 4;
            }
            else return;

            typeOfBall = TypeOfBall.ballWithMonster;
            canThrow = true;
            currentMonsterSlot = i;
            setCurrentPetSlot?.Invoke(currentMonsterSlot);
        }
    }

    private void Update()
    {
        ThrowBallUpdate();
        ChooseSimpleBallUpdate();
        ChoosePetSlotUpdate();
    }
}
