using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

enum EnemyStates
{
    idle, 
    run,
    attack,
    dead,
    goingBack
}

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

    protected int attackCooldownTime = 3;
    protected int abilityCooldown = 6;
    protected float abilityDamage = 40;
    protected float distanceToTarget;

    protected int attackDamage = 5;

    protected Vector3 addHeight = Vector3.up;

    protected bool attackOnCooldown;
    protected bool abilityOnCooldown;
    protected bool iSeePlayer = false;
    protected bool targetIsHuman = true;
    protected bool animating;
    protected bool inBattle;

    private bool goingToStartPosition = false;

    public bool ISeePlayer => iSeePlayer;

    public float DistanceToTarget => distanceToTarget;

    private Vector3 pointWhereStartedAgr;
    private Vector3 pointOfTargetCollider;

    private const string running = "running";

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

        CameraManager.OnMainCharacterSwapped += ChangeTarget;
        CameraManager.OnPlayerIsMainCharacter += TargetIsHuman;
        CameraManager.OnPlayerIsNotMainCharacter += TargetIsNotHuman;
    }

    private void OnEnable()
    {
        pointWhereStartedAgr = transform.position;
    }

    public virtual void OnDisable()
    {
        CameraManager.OnMainCharacterSwapped -= ChangeTarget;
        CameraManager.OnPlayerIsMainCharacter -= TargetIsHuman;
        CameraManager.OnPlayerIsNotMainCharacter -= TargetIsNotHuman;
    }

    public virtual void Update()
    {

        if (goingToStartPosition && Vector3.Distance(pointWhereStartedAgr, transform.position) <= 0.5)
        {
            pointWhereStartedAgr = transform.position;
            goingToStartPosition = false;
        }

        if (!goingToStartPosition)
        {
            if (healthManager.IsDead) return;

            CheckForMostClosePointOfTarget();
            distanceToTarget = Vector3.Distance(pointOfTargetCollider, transform.position);
            if (!animating && target != null)
            {
                if (!inBattle)
                {
                    if (distanceToTarget <= tauntRange)
                    {
                        CheckIfYouSeePlayer();
                    }
                    else
                    {
                        StopMove();
                        if (Vector3.Distance(pointWhereStartedAgr, transform.position) > 0.5f)
                        {
                            goingToStartPosition = true;
                            GoToPoint(pointWhereStartedAgr);
                        }
                    }
                }
            }
            if (goingToStartPosition) return;

            if (!animating && (iSeePlayer || inBattle) && distanceToTarget > +attackRange)
            {
                FollowTarget();
                meshAgent.isStopped = false;
            }
            else
            {
                meshAgent.isStopped = true;
                animator.SetBool(running, false);
            }

            if (targetIsHuman && distanceToTarget <= 3 && !animating)
            {
                MonsterToutchedPlayer();
            }

            if (!targetIsHuman && distanceToTarget <= attackRange && !animating)
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
            AttackCooldown();
        }
    }

    public async void AttackCooldown()
    {
        attackOnCooldown = true;
        await Task.Delay(attackCooldownTime * 1000);
        attackOnCooldown = false;
    }

    public async void AbilityCooldown()
    {
        attackOnCooldown = true;
        await Task.Delay(abilityCooldown * 1000);
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
            if (iSeePlayer == false && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Pet")))
            {
                animator.SetTrigger("taunted");
                ISeePlayerOn();
            }
        }
    }

    public void CheckForMostClosePointOfTarget()
    {
        Vector3 targetPos = target.transform.position + addHeight;
        Vector3 direction = targetPos - pointForRay.position;
        int layerNumber = 6;
        LayerMask layerMask = 1 << layerNumber;
        if (Physics.Raycast(pointForRay.position, direction, out var hit, 100, layerMask))
        {
            if (hit.collider != null)
            {
                pointOfTargetCollider = hit.point;
            }
        }
    }

    public void GoToPoint(Vector3 point)
    {
        meshAgent.isStopped = false;
        meshAgent.destination = point;
    }

    public void ISeePlayerOn()
    {
        iSeePlayer = true;
    }

    public void StopMove()
    {
        iSeePlayer = false;
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
