using UnityEngine;

public class MainCharacterAnimation: MonoBehaviour
{
    private Animator animator;

    private readonly int SitDown = Animator.StringToHash("sitDown");
    private readonly int StandUp = Animator.StringToHash("standUp");
    private readonly int Cheer = Animator.StringToHash("cheer");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        CameraManager.OnPlayerIsMainCharacter += Stand;
        CameraManager.OnPlayerIsNotMainCharacter += Sit;
        BattleManager.winBattle += WinBattle;
        BattleManager.loseBattle += LoseBattle;
    }

    private void OnDisable()
    {
        CameraManager.OnPlayerIsMainCharacter -= Stand;
        CameraManager.OnPlayerIsNotMainCharacter -= Sit;
        BattleManager.winBattle -= WinBattle;
        BattleManager.loseBattle -= LoseBattle;
    }
    private void Sit()
    {
        animator.SetTrigger(SitDown);
    }

    private void Stand()
    {
        animator.SetTrigger(StandUp);
    }

    private void WinBattle()
    {
        animator.SetTrigger(Cheer);
    }

    private void LoseBattle()
    {

    }

}
