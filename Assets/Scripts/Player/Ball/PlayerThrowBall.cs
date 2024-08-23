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
    Transform pointForLaunch;
    GameObject mainCamera;
    [SerializeField]
    private CinemachineFreeLook virtualCamera;

    private bool isMainCharacter = true;
    private bool canThrow = true;

    private TypeOfBall typeOfBall;

    private float vertical;

    private Animator animator;

    private LineRenderer lineRenderer;

    private MonsterSpace monsterSpace;

    private int currentMonsterSlot = 1;

    private Vector3 ballTrajectory;
    private Vector3 ballTrajectoryAdd = new Vector3 (0, 0.5f, 0);

    private void OnEnable()
    {
        CameraManager.mainCharacterSwapped += EnableOrDisableCanThrow;
    }

    private void OnDisable()
    {
        CameraManager.mainCharacterSwapped -= EnableOrDisableCanThrow;
    }

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        monsterSpace = GetComponent<MonsterSpace>();
        animator = GetComponent<Animator>();
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
                Monster monster = monsterSpace.monsterInCurrentSlot(currentMonsterSlot);
                newBall = Instantiate(ballWithMonster, pointForLaunch.position, Quaternion.identity);
                newBall.GetComponent<BallWithMonster>().PutMosnterIn(monster);
                newBall.GetComponent<Rigidbody>().velocity = ballTrajectory * ballForce;
                break;
        }

    }

    private void ShowTrajectory()
    {
        //vertical = Input.GetAxisRaw("Mouse Y");
        //ballForce += vertical * 0.1f;
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

    //private void FreezeCamera()
    //{
    //    virtualCamera.m_YAxis.m_MaxSpeed = 0;
    //}

    //private void DeFreezeCamera()
    //{
    //    virtualCamera.m_YAxis.m_MaxSpeed = 3;
    //}

    private void Update()
    {
        if (isMainCharacter)
        {
            if (canThrow)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    lineRenderer.enabled = true;
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

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (monsterSpace.SlotOneID != null)
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
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (monsterSpace.SlotTwoID != null)
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
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (monsterSpace.SlotThreeID != null)
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
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (monsterSpace.SlotFourID != null)
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
