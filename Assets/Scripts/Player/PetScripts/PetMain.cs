using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMain : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("spawned");
    }



    void Update()
    {
        
    }
}
