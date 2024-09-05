using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        CameraManager.OnPlayerIsMainCharacter += SitUp;
        CameraManager.OnPlayerIsNotMainCharacter += SitDown;
        BattleManager.winBattle += WinBattle;
        BattleManager.loseBattle += LoseBattle;
    }

    private void OnDisable()
    {
        CameraManager.OnPlayerIsMainCharacter -= SitUp;
        CameraManager.OnPlayerIsNotMainCharacter -= SitDown;
        BattleManager.winBattle -= WinBattle;
        BattleManager.loseBattle -= LoseBattle;
    }
    private void SitDown()
    {
        animator.SetTrigger("sitDown");
    }

    private void SitUp()
    {
        animator.SetTrigger("sitUp");
    }

    private void WinBattle()
    {
        animator.SetTrigger("cheer");
    }

    private void LoseBattle()
    {

    }

}
