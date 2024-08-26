using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemManager : AttackManager
{
    [SerializeField]
    private Transform pointForRockCreate;
    [SerializeField]
    private Transform throwPoint;
    [SerializeField]
    private Transform grabPoint;
    [SerializeField]
    private float ballForce;
    private float _ballForce;
    [SerializeField]
    private GameObject rock;

    private float throwRockCooldownTime = 2;
    private bool throwRockOnCooldown = false;

    private Vector3 rockTrajectory;
    private Vector3 rockTrajectoryAdd = new Vector3(0, 0.5f, 0);

    private LineRenderer lineRenderer;

    private GameObject rockGO;

    private bool rockGrabbed = false;
    private bool showTrajectory;


    public override void Start()
    {
        base.Start();
        lineRenderer = GetComponentInChildren<LineRenderer>();        
    }

    public override void Update()
    {
        if (canAttack && playerMoveHumanoid.IsGrounded == true)
        {
            if (!throwRockOnCooldown)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    lineRenderer.enabled = true;
                    playerMoveHumanoid.FreezeThrowRotation();
                    showTrajectory = true;
                }

                if (showTrajectory && Input.GetKey(KeyCode.Mouse0))
                {
                    ShowTrajectory();
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    playerMoveHumanoid.UnFreezeThrowRotation();
                    animator.SetTrigger("attack1");
                    lineRenderer.enabled = false;
                    showTrajectory = false;
                }

                if (rockGO != null && rockGrabbed)
                {
                    GrabRock();
                }
            }
            Attack2();
        }
    }
    
    private void ThorwRockCooldownOff()
    {
        throwRockOnCooldown = false;
    }


    public void ThrowRock()
    {
        rockGrabbed = false;
        rockGO.GetComponent<Rigidbody>().velocity = rockTrajectory * _ballForce;
        _ballForce = ballForce;
        rockGO.GetComponent<Rock>().thrown = true;
        throwRockOnCooldown = true;
        Invoke("ThorwRockCooldownOff", throwRockCooldownTime);
    }

    private void GrabRock()
    {
        rockGO.transform.position = grabPoint.position;
    }

    private void GrabbedOn() => rockGrabbed = true;

    private void CreateRock()
    {
        rockGO = Instantiate(this.rock, pointForRockCreate.position, Quaternion.identity);
        rockGO.GetComponent<Rock>().rockExplosed += DealExplosionDamage;
    }

    private void ShowTrajectory()
    {
        float vertical = Input.GetAxisRaw("Mouse Y");
        _ballForce += vertical * 70 * Time.deltaTime;
        if (_ballForce <= 7) _ballForce = 7;
        if (_ballForce >= 15) _ballForce = 15;
        Vector3[] points = new Vector3[25];
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            rockTrajectory = gameObject.transform.forward;
            rockTrajectory += rockTrajectoryAdd;
            rockTrajectory = rockTrajectory.normalized;
            points[i] = throwPoint.position + rockTrajectory * _ballForce * time + Physics.gravity * time * time / 2f;
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    private void DealExplosionDamage(List<GameObject> targets)
    {

        foreach (GameObject target in targets)
        {

            if (target.CompareTag("Enemy") || target.CompareTag("Monster"))
            {
                DealDamage(attack2Damage, target);
            }
        }
    }
}
