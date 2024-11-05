using System;
using UnityEngine;

public abstract class UnitAttack : MonoBehaviour
{
    public static event Action<GameObject, int> hitTarget;

    protected Animator animator;

    [SerializeField]
    protected Transform simpleAttackPoint;
    [SerializeField]
    protected Transform powerAttackPoint;

    protected PlayerHumanoidMove playerHumanoidMove;

    [SerializeField]
    protected int simpleAttackDamage;
    [SerializeField]
    protected int powerAttackDamage;
    [SerializeField]
    protected float simpleAttack1CooldownTime = 3;
    [SerializeField]
    protected float powerAttackCooldownTime = 5;
    protected bool simpleAttackOnCooldown = false;
    protected bool powerAttackOnCooldown;

    protected bool isAttacking = false;
    protected bool canAttack = true;

    protected readonly int SimpleAttackAnim = Animator.StringToHash("simpleAttack");
    protected readonly int PowerAttackAnim = Animator.StringToHash("powerAttack");

    protected KeyCode keyCodeSimpleAttack = KeyCode.Mouse0;
    protected KeyCode keyCodePowerAttack = KeyCode.Mouse1;


    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        TryGetComponent<PlayerHumanoidMove>(out var playerMoveHumanoid);
        this.playerHumanoidMove = playerMoveHumanoid;
    }

    private void OnEnable()
    {
        CameraManager.OnMainCharacterSwapped += IAmMainCharacter;
    }

    private void OnDisable()
    {
        CameraManager.OnMainCharacterSwapped -= IAmMainCharacter;
    }

    virtual public void Update()
    {
        if (canAttack && playerHumanoidMove.IsGrounded == true)
        {
                SimpleAttackUpdate();

                PowerAttackUpdate();
        }
    }

    private void IAmMainCharacter(GameObject target)
    {
        if (target == gameObject) canAttack = true;
        else canAttack = false;
    }

    private void prohibitAttack() => isAttacking = true;

    private void AllowAttack() => isAttacking = false;

    private void SimpleAttackCooldownOff() => simpleAttackOnCooldown = false;


    private void PowerAttackCooldownOff() => powerAttackOnCooldown = false;

    private void DealSimpleDamage()
    {
        DealDamage(simpleAttackDamage, simpleAttackPoint);
    }

    private void DealPowerDamage()
    {
        DealDamage(powerAttackDamage, powerAttackPoint);
    }

    public void DealDamage(int damage, Transform attackPoint)
    {
        var colliders = Physics.OverlapSphere(attackPoint.position, 0.5f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.GetComponent<IDamageable>() != null && collider.gameObject.CompareTag("Monster"))
            {
                DealDamage(damage, collider.gameObject);
            }
        }
    }

    public void DealDamage(int damage, GameObject target)
    {
        hitTarget?.Invoke(target, damage);
    }

    public virtual void SimpleAttackUpdate()
    {
        if (canAttack && playerHumanoidMove.IsGrounded) 
        {
            if (Input.GetKeyDown(keyCodeSimpleAttack) && !simpleAttackOnCooldown && !isAttacking)
            {
                animator.SetTrigger(SimpleAttackAnim);
                simpleAttackOnCooldown = true;
                Invoke("SimpleAttackCooldownOff", simpleAttack1CooldownTime);
                isAttacking = true;
            }
        }
    }

    public virtual void PowerAttackUpdate()
    {
        if (canAttack && playerHumanoidMove.IsGrounded)
        {
            if (Input.GetKeyDown(keyCodePowerAttack) && !powerAttackOnCooldown && !isAttacking)
            {
                animator.SetTrigger(PowerAttackAnim);
                powerAttackOnCooldown = true;
                Invoke("PowerAttackCooldownOff", powerAttackCooldownTime);
                isAttacking = true;
            }       
        }
    }
}
