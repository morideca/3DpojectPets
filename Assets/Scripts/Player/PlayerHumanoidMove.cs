using UnityEngine;

public class PlayerHumanoidMove : MonoBehaviour
{
    private CharacterController controller;

    private Animator animator;

    private Transform mainCamera;

    [SerializeField]
    private float runSpeed = 6;
    [SerializeField]
    private float walkSpeed = 3;
    private float moveSpeed;
    private float smoothTime = 0.1f;
    private float smoothVelocity;
    private float gravityForce;
    private float jumpTime = 1;
    private float gravity = 9.8f;
    private float jumpForce;

    private bool canMove = true;
    private bool isJumping = false;
    private bool freezeThrowRotation = false;

    [SerializeField]
    private bool iAmMainCharacter;
    [SerializeField]
    private bool allowRun;
    [SerializeField]
    private bool allowWalk;

    public bool IsGrounded => controller.isGrounded;

    private readonly int Run = Animator.StringToHash("running");
    private readonly int Walk = Animator.StringToHash("walking");
    private readonly int Jump = Animator.StringToHash("jumpStart");
    private readonly int Land = Animator.StringToHash("jumpLand");

    private void OnEnable()
    {
        CameraManager.OnMainCharacterSwapped += StopOrStartMove;
        HealthManager.petDied += StopMove;
    }

    private void OnDisable()
    {
        CameraManager.OnMainCharacterSwapped -= StopOrStartMove;
        HealthManager.petDied -= StopMove;
    }

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main.transform;

        if (iAmMainCharacter && CameraManager.SceneType == SceneType.battle)
        {
            canMove = false;
        }
        jumpForce = jumpTime * gravity / 2;
        moveSpeed = walkSpeed;
    }

    public void Update()
    {
        if (canMove)
        {
            UpdateJump();
            UpdateMove();
        }
        UpdateGravity();
    }

    private void UpdateGravity()
    {
        gravityForce -= gravity * Time.deltaTime;
        Vector3 move = new Vector3(0, 0, 0);
        move.y += gravityForce;
        controller.Move(move * Time.deltaTime);
    }

    public void StopMove()
    {
        canMove = false;
    }

    public void StartMove()
    {
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

    private void UpdateMove()
    {
        Vector3 move = new Vector3();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0)
        {
            if (!animator.GetBool(Run))
            {
                animator.SetBool(Walk, true);
            }

            if (allowRun && Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool(Walk, false);
                animator.SetBool(Run, true);
                moveSpeed = runSpeed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                animator.SetBool(Walk, true);
                animator.SetBool(Run, false);
                moveSpeed = walkSpeed;
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
            animator.SetBool(Walk, false);
            animator.SetBool(Run, false);
        }

        controller.Move(Vector3.up * gravityForce * Time.deltaTime + move * moveSpeed * Time.deltaTime);
    }

    private void UpdateJump()
    {
        if (IsGrounded)
        {
            if (isJumping)
            {
                animator.SetTrigger(Land);
                isJumping = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                gravityForce = jumpForce;
                animator.SetTrigger(Jump);
                isJumping = true;
            }
        }
    }

    public void FreezeRotation() => freezeThrowRotation = true;
    public void UnFreezeRotation() => freezeThrowRotation = false;
}
