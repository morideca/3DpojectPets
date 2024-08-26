using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWithMonster : MonoBehaviour
{
    private GameObject monster;
    private Rigidbody rb;
    private bool summoned = false;

    public static event Action<GameObject> petSummoned;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PutMosnterIn(Monster monster)
    {
        this.monster = monster.GOPet;
    }

    private void Update()
    {
        if (rb.velocity.magnitude <= 0.5 && summoned == false)
        {
            summoned = true;
            var monster = Instantiate(this.monster, transform.position, Quaternion.identity);
            petSummoned?.Invoke(monster);
            Destroy(gameObject);
        }
    }
}
