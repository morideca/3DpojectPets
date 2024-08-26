using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterAI : MonoBehaviour
{
    public static event Action monsterToutchedPlayer;
    public static event Action<GameObject, int> monsterAttackedPet;

    protected HealthManager healthManager;

    protected GameObject target;
    protected GameObject player;

    protected Animator animator;

    protected NavMeshAgent meshAgent;

    [SerializeField]
    protected Transform pointForRay;

    [SerializeField]
    protected LayerMask wallLayer;

    [SerializeField]
    protected float tauntRange;
    [SerializeField]
    protected float attackRange;
    protected float attackCooldownTime = 3;
    protected float abilityCooldown = 6;
    protected float abilityDamage = 40;
    protected float distanceToTarget;
    protected int attackDamage = 5;
    protected Vector3 addHeight = Vector3.up;

    protected bool attackOnCooldown;
    protected bool abilityOnCooldown;
    protected bool tauntOn = false;
    protected bool targetIsHuman = true;
    protected bool animating;
    protected bool inBattle;

    public bool IsTaunted => tauntOn;

    public virtual void Start()
    {
        if (CameraManager.SceneType == SceneType.battle)
        {
            inBattle = true;
            targetIsHuman = false;
        }
        else inBattle = false;
        if (CameraManager.SceneType == SceneType.main)
        {
            inBattle = false;
        }

        target = CameraManager.cameraTarget.transform.root.gameObject;
        if (target == null) target = CameraManager.cameraTarget;
        animator = GetComponent<Animator>();
        healthManager = GetComponent<HealthManager>();
        meshAgent = GetComponent<NavMeshAgent>();

        CameraManager.mainCharacterSwapped += ChangeTarget;
        CameraManager.playerIsMainCharacter += TargetIsHuman;
        CameraManager.playerIsNotMainCharacter += TargetIsNotHuman;
    }

    public virtual void OnDisable()
    {
        CameraManager.mainCharacterSwapped -= ChangeTarget;
        CameraManager.playerIsMainCharacter -= TargetIsHuman;
        CameraManager.playerIsNotMainCharacter -= TargetIsNotHuman;
    }

    public virtual void Update()
    {
        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (!healthManager.IsDead)
        {
            if (!animating && target != null)
            {
                if (!inBattle)
                {
                    distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
                    if (distanceToTarget <= tauntRange)
                    {
                        CheckIfYouSeePlayer();
                    }
                   else StopMove();
                }
            }

            if (!animating && (tauntOn || inBattle))
            {
                FollowTarget();
                meshAgent.isStopped = false;
            }
            else
            {
                meshAgent.isStopped = true;
                animator.SetBool("running", false);
            }

            if (targetIsHuman && distanceToTarget <= 3 && !animating)
            {
                MonsterToutchedPlayer();
            }
            else if (distanceToTarget <= attackRange && !animating)
            {
                Attack();
            }
        }
    }

    public void FollowTarget()
    {
        animator.SetBool("running", true);
        if (!healthManager.IsDead) meshAgent.destination = target.transform.position;
        else meshAgent.isStopped = true;
    }

    public void MonsterToutchedPlayer()
    {
        monsterToutchedPlayer?.Invoke();
    }

    public void Attack()
    {
        if (!attackOnCooldown)
        {
            animator.SetTrigger("attack");
            attackOnCooldown = true;
            Invoke("attackCooldownOff", attackCooldownTime);
        }
    }

    public void attackCooldownOff()
    {
        attackOnCooldown = false;
    }

    public void abilityCooldownOff()
    {
        abilityOnCooldown = false;
    }

    public void DealDamage()
    {
        monsterAttackedPet?.Invoke(target, attackDamage);
    }

    public void DealDamage(int damage, GameObject target)
    {
        monsterAttackedPet?.Invoke(target, damage);
    }

    public void ChangeTarget(GameObject target)
    {
        this.target = target;
    }

    public void CheckIfYouSeePlayer()
    {
        Vector3 targetPos = target.transform.position + addHeight;
        Vector3 direction = targetPos - pointForRay.position;
        if (Physics.Raycast(pointForRay.position, direction, out var hit, wallLayer))
        {
            if (tauntOn == false && hit.collider.CompareTag("Player"))
            {
                animator.SetTrigger("taunted");
                TauntOn();
            }
        }
    }

    public void TauntOn()
    {
        tauntOn = true;
    }

    public void StopMove()
    {
        tauntOn = false;
        meshAgent.isStopped = true;
    }

    public void AnimatingOn()
    {
        animating = true;
    }

    public void AnimatingOff()
    {
        animating = false;
    }

    public void TargetIsHuman()
    {
        targetIsHuman = true;
    }

    public void TargetIsNotHuman()
    {
        targetIsHuman = false;
    }
}
