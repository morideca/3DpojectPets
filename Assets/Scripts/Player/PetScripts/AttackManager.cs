using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AttackManager : MonoBehaviour
{
    public static event Action<GameObject, int> hitTarget;

    protected Animator animator;

    [SerializeField]
    protected Transform simpleAttackPoint;

    protected PlayerMoveHumanoid playerMoveHumanoid;

    [SerializeField]
    protected int attack1Damage;
    [SerializeField]
    protected int attack2Damage;
    [SerializeField]
    protected float attack1CooldownTime = 3;
    [SerializeField]
    protected float attack2CooldownTime = 5;
    protected bool attack1OnCooldown = false;
    protected bool Attack2OnCooldown;

    protected bool isAttacking = false;
    protected bool canAttack = true;


    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        TryGetComponent<PlayerMoveHumanoid>(out var playerMoveHumanoid);
        this.playerMoveHumanoid = playerMoveHumanoid;
    }

    private void OnEnable()
    {
        CameraManager.OnMainCharacterSwapped += CheckForNewCameraTarget;
    }

    private void OnDisable()
    {
        CameraManager.OnMainCharacterSwapped -= CheckForNewCameraTarget;
    }



    virtual public void Update()
    {
        if (canAttack && playerMoveHumanoid.IsGrounded == true)
        {
                Attack1();

                Attack2();
        }
    }

    private void CheckForNewCameraTarget(GameObject target)
    {
        if (target == gameObject) canAttack = true;
        else canAttack = false;
    }

    private void IsAttackingOff()
    {
        isAttacking = false;
    }

    private void Attack1OnCooldownOff() => attack1OnCooldown = false;


    private void Attack2CooldownOff() => Attack2OnCooldown = false;



    private void DealSimpleDamage()
    {
        DealDamage(attack1Damage);
    }

    private void DealPowerDamage()
    {
        DealDamage(attack2Damage);
    }

    public void DealDamage(int damage)
    {
        var colliders = Physics.OverlapSphere(simpleAttackPoint.position, 0.5f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.GetComponent<IDamageable>() != null && collider.gameObject.CompareTag("Monster"))
            {
                hitTarget?.Invoke(collider.gameObject, damage);
            }
        }
    }

    public void DealDamage(int damage, GameObject target)
    {
        hitTarget?.Invoke(target, damage);
    }

    virtual public void Attack1()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !attack1OnCooldown && !isAttacking)
        {
            animator.SetTrigger("attack1");
            attack1OnCooldown = true;
            Invoke("Attack1OnCooldownOff", attack1CooldownTime);
            isAttacking = true;
        }
    }

    virtual public void Attack2()
    {
    if (Input.GetKeyDown(KeyCode.Mouse1) && !Attack2OnCooldown && !isAttacking)
    {
            animator.SetTrigger("attack2");
        Attack2OnCooldown = true;
        Invoke("Attack2CooldownOff", attack2CooldownTime);
        isAttacking = true;
    }
    }


}
