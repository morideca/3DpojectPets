using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject ballWithMonster;
    [SerializeField]
    private float ballForce;
    [SerializeField]
    private Transform pointForLaunch;
    private GameObject mainCamera;
    [SerializeField]
    private CinemachineFreeLook virtualCamera;

    private bool isMainCharacter = true;
    private bool canThrow = true;

    private static int currentMonsterSlot = 1;

    public static int CurrentMosnterSlot => currentMonsterSlot;

    private float vertical;

    private Vector3 ballTrajectory;
    private Vector3 ballTrajectoryAdd = new Vector3(0, 0.5f, 0);

    private TypeOfBall typeOfBall;

    private Animator animator;
    private LineRenderer lineRenderer;
    private MonsterSpace monsterSpace;
    private PlayerMoveHumanoid playerMoveHumanoid;





    private void OnEnable()
    {
        CameraManager.OnMainCharacterSwapped += EnableOrDisableCanThrow;
    }

    private void OnDisable()
    {
        CameraManager.OnMainCharacterSwapped -= EnableOrDisableCanThrow;
    }

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        monsterSpace = GetComponent<MonsterSpace>();
        animator = GetComponent<Animator>();
        playerMoveHumanoid = GetComponent<PlayerMoveHumanoid>();
    }

    private void EnableOrDisableCanThrow(GameObject target)
    {
        if (target == this.gameObject)
        {
            isMainCharacter = true;
        }
        else isMainCharacter = false;
    }

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
        if (ballForce <= 7) ballForce = 7;
        if (ballForce >= 15) ballForce = 15;
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

    private void Update()
    {
        if (isMainCharacter)
        {
            if (canThrow)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    lineRenderer.enabled = true;
                    playerMoveHumanoid.FreezeThrowRotation();
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
            if (Input.GetKeyDown(KeyCode.C))
            {
                setCurrentPetSlot?.Invoke(0);
                typeOfBall = TypeOfBall.simpleBall;
                canThrow = true;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && monsterSpace.PetInSlots.Count > 0)
            {
                if (monsterSpace.PetInSlots[0] != null)
                {
                    typeOfBall = TypeOfBall.ballWithMonster;
                    canThrow = true;
                }
                else
                {
                    Debug.Log("слот пустой");
                    canThrow = false;
                }
                currentMonsterSlot = 1;
                setCurrentPetSlot?.Invoke(currentMonsterSlot);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && monsterSpace.PetInSlots.Count > 0)
            {
                if (monsterSpace.PetInSlots[1] != null)
                {
                    typeOfBall = TypeOfBall.ballWithMonster;
                    canThrow = true;
                }
                else
                {
                    Debug.Log("слот пустой");
                    canThrow = false;
                }
                currentMonsterSlot = 2;
                setCurrentPetSlot?.Invoke(currentMonsterSlot);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && monsterSpace.PetInSlots.Count > 0)
            {
                if (monsterSpace.PetInSlots[2] != null)
                {
                    typeOfBall = TypeOfBall.ballWithMonster;
                    canThrow = true;
                }
                else
                {
                    Debug.Log("слот пустой");
                    canThrow = false;
                }
                currentMonsterSlot = 3;
                setCurrentPetSlot?.Invoke(currentMonsterSlot);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && monsterSpace.PetInSlots.Count > 0)
            {
                if (monsterSpace.PetInSlots[3] != null)
                {
                    typeOfBall = TypeOfBall.ballWithMonster;
                    canThrow = true;
                }
                else
                {
                    Debug.Log("слот пустой");
                    canThrow = false;
                }
                currentMonsterSlot = 4;
                setCurrentPetSlot?.Invoke(currentMonsterSlot);
            }
        }
    }
}
