using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWithMonster : MonoBehaviour
{
    private Pet pet;
    private Rigidbody rb;
    private bool summoned = false;

    public static event Action<GameObject> petSummoned;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PutMosnterIn(Pet pet)
    {
        this.pet = pet;
        Debug.Log(this.pet.CurrentHP);

    }

    private void Update()
    {
        if (rb.velocity.magnitude <= 0.5 && summoned == false)
        {
            summoned = true;
            var pet = Instantiate(this.pet.GOPet, transform.position, Quaternion.identity);
            var healthManager = pet.GetComponent<HealthManager>();
            healthManager.SetCurrentHealth(this.pet.CurrentHP);
            healthManager.SetMaxHealth(this.pet.MaxHP);
            petSummoned?.Invoke(pet);
            Destroy(gameObject);
        }
    }
}
