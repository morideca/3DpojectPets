using System;
using UnityEngine;

public class BallWithMonster : MonoBehaviour
{
    private Pet pet;
    private Rigidbody rb;
    private ServiceLocator serviceLocator;
    private EventManager eventManager;

    private bool released = false;

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        eventManager = serviceLocator.EventManager; 
    }
    private void Update()
    {
        ReleasePetUpdate();
    }

    public void PutMosnterIn(Pet pet)
    {
        this.pet = pet;
    }

    private void ReleasePetUpdate()
    {
        if (rb.velocity.magnitude <= 0.5 && !released)
        {
            released = true;
            var pet = Instantiate(this.pet.GOPet, transform.position, Quaternion.identity);
            var healthManager = pet.GetComponent<HealthManager>();
            healthManager.SetPetClass(this.pet);
            eventManager.TriggerPetSummoned(pet);
            serviceLocator.SetPet(pet);
            Destroy(gameObject);
        }
    }
}
