using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public static event Action monsterToutchedPlayer;
    public static event Action<GameObject, int> monsterAttackedPet;

    private HealthManager healthManager;

    private GameObject target;
    private GameObject player;

    private Animator animator;
    private NavMeshAgent meshAgent;

    [SerializeField]
    private Transform pointForRay;
    [SerializeField]
    private float speed;
    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField]
    private float tauntRange;
    [SerializeField]
    private float attackRange;
    private float attackCooldownTime = 3;
    private bool attackOnCooldown;
    private int attackDamage = 5;
    private Vector3 addHeight = Vector3.up;

    private float distanceToTarget;
   

    private bool tauntOn = false;
    private bool targetIsHuman = false;
    private bool animating;

    private bool inBattle;


    public bool IsTaunted => tauntOn;

    void Start()
    {
        if (CameraManager.SceneType == SceneType.battle) inBattle = true;
        else inBattle = false;

        animator = GetComponent<Animator>();
        target = CameraManager.cameraTarget;

        healthManager = GetComponent<HealthManager>();
        meshAgent = GetComponent<NavMeshAgent>();

        CameraManager.mainCharaterSwapped += ChangeTarget;
        CameraManager.playerIsMainCharacter += TargetIsHuman;
        CameraManager.playerIsNotMainCharacter += TargetIsNotHuman;
    }


    private void OnDisable()
    {
        CameraManager.mainCharaterSwapped -= ChangeTarget;
        CameraManager.playerIsMainCharacter -= TargetIsHuman;
        CameraManager.playerIsNotMainCharacter -= TargetIsNotHuman;
    }

    public void Update()
    {if (!healthManager.IsDead)
            distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
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
                }
                else tauntOn = true;
            }

            if (!animating && tauntOn)
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

    private void MonsterToutchedPlayer()
    {
        monsterToutchedPlayer?.Invoke();
    }

    private void Attack()
    {
        if (!attackOnCooldown)
        {
            Debug.Log("attack!");
            animator.SetTrigger("attack");
            attackOnCooldown = true;
            Invoke("attackCooldownOff", attackCooldownTime);
        }
    }

    private void attackCooldownOff()
    {
        attackOnCooldown = false;
    }

    private void DealDamage()
    {
        monsterAttackedPet?.Invoke(target, attackDamage);
    }

    private void ChangeTarget(GameObject target)
    {
        this.target = target;
    }

    private void CheckIfYouSeePlayer()
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

    private void TauntOn()
    {
        tauntOn = true;
    }
    
    private void AnimatingOn()
    {
        animating = true;
    }

    private void AnimatingOff()
    {
        animating = false;
    }
    
    private void TargetIsHuman()
    {
        targetIsHuman = true;
    }

    private void TargetIsNotHuman()
    {
        targetIsHuman = false;
    }
}
