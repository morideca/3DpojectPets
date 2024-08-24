using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AttackManager : MonoBehaviour
{
    public static event Action<GameObject, int> hittedTarget;

    private Animator animator;

    [SerializeField]
    private Transform simpleAttackPoint;

    private PlayerMoveHumanoid playerMoveHumanoid;

    [SerializeField]
    protected int attack1;
    [SerializeField]
    protected int attack2;
    [SerializeField]
    private float attack1CooldownTime = 3;
    [SerializeField]
    private float attack2CooldownTime = 5;
    private bool attack1OnCooldown = false;
    private bool Attack2OnCooldown;

    private bool isAttacking = false;
    private bool canAttack = true;


    void Start()
    {
        animator = GetComponent<Animator>();
        TryGetComponent<PlayerMoveHumanoid>(out var playerMoveHumanoid);
        this.playerMoveHumanoid = playerMoveHumanoid;
    }

    private void OnEnable()
    {
        CameraManager.mainCharacterSwapped += CheckForNewCameraTarget;
    }

    private void OnDisable()
    {
        CameraManager.mainCharacterSwapped -= CheckForNewCameraTarget;
    }

    private void CheckForNewCameraTarget(GameObject target)
    {
        if (target == gameObject) canAttack = true;
        else canAttack = false;
    }

    void Update()
    {
        Debug.Log(canAttack);
        Debug.Log(isAttacking);
        Debug.Log(attack1OnCooldown);
        Debug.Log(playerMoveHumanoid.IsGrounded);
        if (canAttack && playerMoveHumanoid.IsGrounded == true)
        {
                Attack1();

                Attack2();
        }
    }

    private void IsAttackingOff()
    {
        isAttacking = false;
    }

    private void attack1OnCooldownOff() => attack1OnCooldown = false;


    private void attack2CooldownOff() => Attack2OnCooldown = false;



    private void dealSimpleDamage()
    {
        dealDamage(attack1);
    }

    private void dealPowerDamage()
    {
        dealDamage(attack2);
    }

    public virtual void dealDamage(int damage)
    {
        var colliders = Physics.OverlapSphere(simpleAttackPoint.position, 0.5f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.GetComponent<IDamageable>() != null && collider.gameObject.CompareTag("Monster"))
            {
                hittedTarget?.Invoke(collider.gameObject, damage);
                Debug.Log(collider.gameObject);
            }
        }
    }

    virtual public void Attack1()
    {
        Debug.Log("attack");
        if (Input.GetKeyDown(KeyCode.Mouse0) && !attack1OnCooldown && !isAttacking)
        {
            Debug.Log("attack");
            animator.SetTrigger("attack1");
            attack1OnCooldown = true;
            Invoke("attack1OnCooldownOff", attack1CooldownTime);
            isAttacking = true;
        }
    }

    virtual public void Attack2()
    {
    if (Input.GetKeyDown(KeyCode.Mouse1) && !Attack2OnCooldown && !isAttacking)
    {
            Debug.Log("attack");
            animator.SetTrigger("attack2");
        Attack2OnCooldown = true;
        Invoke("attack2CooldownOff", attack2CooldownTime);
        isAttacking = true;
    }
    }


}
