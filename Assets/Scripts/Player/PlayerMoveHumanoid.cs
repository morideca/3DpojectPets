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
    private float walkSpeed = 4;

    private float force;
    private float smoothTime = 0.1f;
    private float smoothVelocity;
    private float jumpForce;
    private float yVelocity;
    private float jumpTime = 1;
    private float gravity = 3;

    private bool canMove = true;
    private bool isJumping = false;

    public bool IsGrounded => controller.isGrounded;

    private void OnEnable()
    {
        CameraManager.mainCharaterSwapped += StopOrStartMove;
        AttackManager.attack += StopMove;
        AttackManager.endAttack += StartMove;
        HealthManager.petDied += StopMove;
    }

    private void OnDisable()
    {
        CameraManager.mainCharaterSwapped -= StopOrStartMove;
        AttackManager.attack -= StopMove;
        AttackManager.endAttack -= StartMove;
        HealthManager.petDied -= StopMove;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
        jumpForce = jumpTime * gravity / 2;
    }

    public void StopMove()
    {
        if (this.gameObject == GameManager.CurrentMainCharacter)
        canMove = false;
    }

    public void StartMove()
    {
        if (this.gameObject == GameManager.CurrentMainCharacter)
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



    private void Update()
    {
        if (canMove)
        {
            Vector3 move = new Vector3();

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude > 0.1f)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetBool("walking", false);
                    animator.SetBool("running", true);
                    force = runSpeed;
                }
                else
                {
                    animator.SetBool("walking", true);
                    animator.SetBool("running", false);
                    force = walkSpeed;
                }

                float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref smoothVelocity, smoothTime);
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    float angle1 = Mathf.SmoothDampAngle(transform.eulerAngles.y, mainCamera.eulerAngles.y, ref smoothVelocity, smoothTime / 10);
                    transform.rotation = Quaternion.Euler(0f, angle1, 0f);
                }
                else transform.rotation = Quaternion.Euler(0f, angle, 0f);
                move = (Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward).normalized;
            }
            else
            {
                animator.SetBool("walking", false);
                animator.SetBool("running", false);
            }

            if (controller.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    yVelocity = jumpForce;
                    animator.SetTrigger("jumpStart");
                    isJumping = true;
                }
            }
            else
            {
                yVelocity -= gravity * Time.deltaTime;
            }

            move.y += yVelocity;
            controller.Move(move * force * Time.deltaTime);

            if (isJumping && controller.isGrounded)
            {
                animator.SetTrigger("jumpLand");
                isJumping = false;
            }
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;
            Vector3 move = new Vector3(0, 0, 0);
            move.y += yVelocity;
            controller.Move(move * Time.deltaTime);
        }
    }
}
