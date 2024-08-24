using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWithMonster : MonoBehaviour
{
    private GameObject monster;
    private Rigidbody rb;

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
        if (rb.velocity.magnitude <= 0.5)
        {
            var _monster = Instantiate(monster, transform.position, Quaternion.identity);
            petSummoned?.Invoke(_monster);
            Destroy(gameObject);
        }
    }
}
