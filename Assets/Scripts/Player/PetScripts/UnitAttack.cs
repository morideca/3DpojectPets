using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class UnitAttack : MonoBehaviour
{
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
    protected int simpleAttack1CooldownTime = 3;
    [SerializeField]
    protected int powerAttackCooldownTime = 5;
    protected bool simpleAttackOnCooldown = false;
    protected bool powerAttackOnCooldown;

    protected bool isAttacking = false;
    protected bool canAttack = true;

    protected readonly int SimpleAttackAnim = Animator.StringToHash("simpleAttack");
    protected readonly int PowerAttackAnim = Animator.StringToHash("powerAttack");

    protected KeyCode keyCodeSimpleAttack = KeyCode.Mouse0;
    protected KeyCode keyCodePowerAttack = KeyCode.Mouse1;

    private ServiceLocator serviceLocator;
    private EventManager eventManager;

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
    }

    public virtual void Start()
    {
        
        animator = GetComponent<Animator>();
        TryGetComponent<PlayerHumanoidMove>(out var playerMoveHumanoid);
        this.playerHumanoidMove = playerMoveHumanoid;
    }

    private void OnEnable()
    {
        if (eventManager == null) eventManager = serviceLocator.EventManager;
        //    CameraManager.OnMainCharacterSwapped += IAmMainCharacter;
    }

    //private void OnDisable()
    //{
    //    CameraManager.OnMainCharacterSwapped -= IAmMainCharacter;
    //}

    virtual public void Update()
    {
        if (canAttack && playerHumanoidMove.IsGrounded == true)
        {
            SimpleAttackUpdate();

            PowerAttackUpdate();
        }
    }

    //private void IAmMainCharacter(GameObject target)
    //{
    //    if (target == gameObject) canAttack = true;
    //    else canAttack = false;
    //}

    private void prohibitAttack() => isAttacking = true;

    private void AllowAttack() => isAttacking = false;

    private async void SimpleAttackCooldown()
    {
        simpleAttackOnCooldown = true;
        await Task.Delay(simpleAttack1CooldownTime * 1000);
        simpleAttackOnCooldown = false; 
    }

    private async void PowerAttackCooldown()
    {
        powerAttackOnCooldown = true;
        await Task.Delay(powerAttackCooldownTime * 1000);
        powerAttackOnCooldown = false;
    }

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
        eventManager.TriggerPlayerAttackEnemy(target, damage);
    }

    public virtual void SimpleAttackUpdate()
    {
        if (canAttack && playerHumanoidMove.IsGrounded) 
        {
            if (Input.GetKeyDown(keyCodeSimpleAttack) && !simpleAttackOnCooldown && !isAttacking)
            {
                animator.SetTrigger(SimpleAttackAnim);
                SimpleAttackCooldown();
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
                PowerAttackCooldown();
                isAttacking = true;
            }       
        }
    }
}
