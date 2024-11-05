using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAI : MonsterAI
{
    [SerializeField]
    private Transform pointForRockCreate;
    [SerializeField]
    private Transform throwPoint;
    [SerializeField]
    private Transform grabPoint;
    [SerializeField]
    private GameObject rock;
    private GameObject rockGO;

    private int rockDamage = 25;

    [SerializeField]
    private float distanceForAbility;

    private bool rockGrabbed = false;

    public override void Update()
    {
        base.Update();

        if (!targetIsHuman && !animating && distanceToTarget < distanceForAbility && distanceToTarget > 3 && abilityOnCooldown == false)
        {
            UseAbility();
        }

        if (!targetIsHuman && rockGrabbed)
        {
            GrabRock();
        }
    }

    public void UseAbility()
    {
        animator.SetTrigger("ability");
        abilityOnCooldown = true;
        Invoke("AbilityCooldownOff", abilityCooldown);
    }


    private void CreateRock()
    {
        rockGO = Instantiate(rock, pointForRockCreate.position, Quaternion.identity);
        rockGO.GetComponent<Rock>().rockExplosed += DealExplosionDamage;
    }

    private void DealExplosionDamage(List<GameObject> targets)
    {
        foreach (GameObject target in targets)
        {
            Debug.Log("dealDamage" + target);
            if ( target.CompareTag("Pet")) 
            {
                DealDamage(rockDamage, target);
            }
        }
    }

    private void GrabRock()
    {
        rockGO.transform.position = grabPoint.position;
    }

    private void GrabbedOn() => rockGrabbed = true;

    public void ThrowRock()
    {
        rockGrabbed = false;
        rockGO.GetComponent<Rock>().ThrownOn();
        Vector3 directionToTarget = target.transform.position - throwPoint.transform.position;
        float distance = Vector3.Distance(target.transform.position, throwPoint.transform.position);
        float angle = Mathf.Deg2Rad * 30;
        Vector3 velocity = new Vector3(directionToTarget.x, distance * Mathf.Tan(angle), directionToTarget.z).normalized;
        rockGO.GetComponent<Rigidbody>().velocity = velocity * rockForce(distance);
    }

    public float rockForce(float distance)    
    {
        float gravity = Physics.gravity.y * -1;
        float angleInRadians = 30 * Mathf.Deg2Rad;
        float launchSpeed = Mathf.Sqrt(((distance - 2f) * gravity) / Mathf.Sin(2 * angleInRadians));
        return launchSpeed;
    }
}
