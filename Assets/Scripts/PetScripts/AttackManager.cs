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

    protected int simpleAttackDamage = 10;
    protected int powerAttackDamage = 30;

    private float simpleAttackCooldownTime = 3;
    private float powerAttackCooldownTime = 5;
    private bool simpleAttackOnCooldown;
    private bool powerAttackOnCooldown;

    private bool isAttacking;
    private bool canAttack;


    void Start()
    {
        animator = GetComponent<Animator>();
        playerMoveHumanoid = GetComponent<PlayerMoveHumanoid>();
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
        if (canAttack && playerMoveHumanoid.IsGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !simpleAttackOnCooldown && !isAttacking)
            {
                SimpleAttack();
                simpleAttackOnCooldown = true;
                Invoke("SimpleAttackCooldownOff", simpleAttackCooldownTime);
                isAttacking = true;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1) && !powerAttackOnCooldown && !isAttacking)
            {
                PowerAttack();
                powerAttackOnCooldown = true;
                Invoke("PowerAttackCooldownOff", powerAttackCooldownTime);
                isAttacking = true;
            }
        }
    }

    private void IsAttackingOff()
    {
        isAttacking = false;
    }

    private void SimpleAttackCooldownOff()
    {
        simpleAttackOnCooldown = false;
    }

    private void PowerAttackCooldownOff()
    {
        powerAttackOnCooldown = false;
    }


    private void dealSimpleDamage()
    {
        dealDamage(simpleAttackDamage);
    }

    private void dealPowerDamage()
    {
        dealDamage(powerAttackDamage);
    }

    public void dealDamage(int damage)
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

    virtual public void SimpleAttack()
    {
        animator.SetTrigger("simpleAttack");
    }

    virtual public void PowerAttack()
    {
        animator.SetTrigger("powerAttack");
    }


}
