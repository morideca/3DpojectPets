using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveHumanoid : MonoBehaviour
{
    private CharacterController controller;

    private Animator animator;

    private Transform mainCamera;

    [SerializeField]
    private float runSpeed = 6;
    [SerializeField]
    private float walkSpeed;

    private float force;
    private float smoothTime = 0.1f;
    private float smoothVelocity;

    private float yVelocity;
    private float jumpTime = 1;
    private float gravity = 9.8f;
    private float jumpForce;

    private bool canMove = true;
    private bool isJumping = false;
    private bool freezeThrowRotation = false;

    [SerializeField]
    private bool allowJump;
    [SerializeField]
    private bool allowRun;
    [SerializeField]
    private bool allowWalk;

    public bool IsGrounded => controller.isGrounded;

    private void OnEnable()
    {
        CameraManager.mainCharacterSwapped += StopOrStartMove;
        HealthManager.petDied += StopMove;
    }

    private void OnDisable()
    {
        CameraManager.mainCharacterSwapped -= StopOrStartMove;
        HealthManager.petDied -= StopMove;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
        jumpForce = jumpTime * gravity / 2;
        force = walkSpeed;
    }

    private void Update()
    {
        if (canMove)
        {
            if (allowJump)
            {
                Jump();
            }

            if (allowWalk) Walk();
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;
            Vector3 move = new Vector3(0, 0, 0);
            move.y += yVelocity;
            controller.Move(move * Time.deltaTime);
        }
    }

    public void StopMove()
    {
       // if (this.gameObject == CameraManager.cameraTarget)
        canMove = false;
    }

    public void StartMove()
    {
       // if (this.gameObject == CameraManager.cameraTarget)
        canMove = true;
    }

    public void StopOrStartMove(GameObject target)
    {
        if (target == this.gameObject)
        {
            canMove = true;
        }
        else canMove = false;
    }

    private void Run()
    {

    }

    private void Walk()
    {
        Vector3 move = new Vector3();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            if (!animator.GetBool("running")) animator.SetBool("walking", true);

            if (allowRun && Input.GetKeyDown(KeyCode.LeftShift))
            {
                animator.SetBool("walking", false);
                animator.SetBool("running", true);
                force = runSpeed;
            }
            if (allowRun && Input.GetKeyUp(KeyCode.LeftShift))
            {
                animator.SetBool("walking", true);
                animator.SetBool("running", false);
                force = walkSpeed;
            }

            
            float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref smoothVelocity, smoothTime);
            if (freezeThrowRotation)
            {
                float angle1 = Mathf.SmoothDampAngle(transform.eulerAngles.y, mainCamera.eulerAngles.y, ref smoothVelocity, smoothTime / 10);
                transform.rotation = Quaternion.Euler(0f, angle1, 0f);
            }
            else transform.rotation = Quaternion.Euler(0f, angle, 0f);
            move = (Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward).normalized;
        }
        else
        {
            if (allowWalk) animator.SetBool("walking", false);
            if (allowRun) animator.SetBool("running", false);
        }
        if (!IsGrounded) yVelocity -= gravity * Time.deltaTime;
        //move.y += yVelocity;
     //   controller.Move(Vector3.down * yVelocity * Time.deltaTime);
        controller.Move(Vector3.up * yVelocity * Time.deltaTime + move * force * Time.deltaTime);
    }

    private void Jump()
    {
        if (IsGrounded && isJumping)
        {
            animator.SetTrigger("jumpLand");
            isJumping = false;
        }

        if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            yVelocity = jumpForce;
            animator.SetTrigger("jumpStart");
            isJumping = true;
        }
    }

    public void FreezeThrowRotation() => freezeThrowRotation = true;
    public void UnFreezeThrowRotation() => freezeThrowRotation = false;


}
