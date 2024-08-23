using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
       CameraManager.playerIsMainCharacter += SitUp;
       CameraManager.playerIsNotMainCharacter += SitDown;
    }

    private void OnDisable()
    {
        CameraManager.playerIsMainCharacter -= SitUp;
        CameraManager.playerIsNotMainCharacter -= SitDown;
    }
    private void SitDown()
    {
        animator.SetTrigger("sitDown");
    }

    private void SitUp()
    {
        animator.SetTrigger("sitUp");
    }

}
