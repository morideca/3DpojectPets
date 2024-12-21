using UnityEngine;

public class MainCharacterAnimation: MonoBehaviour
{
    private Animator animator;

    private ServiceLocator serviceLocator;
    private EventManager eventManager;

    private readonly int SitDown = Animator.StringToHash("sitDown");
    private readonly int StandUp = Animator.StringToHash("standUp");
    private readonly int Cheer = Animator.StringToHash("cheer");

    private void Awake()
    {
        serviceLocator = ServiceLocator.GetInstance();
        eventManager = serviceLocator.EventManager;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (eventManager == null) eventManager = serviceLocator.EventManager;
        eventManager.OnPlayerBecameMainCharacter += Stand;
        eventManager.OnPetBecameMainCharacter += Sit;
        BattleManager.winBattle += WinBattle;
        BattleManager.loseBattle += LoseBattle;
    }

    private void OnDisable()
    {
        eventManager.OnPlayerBecameMainCharacter -= Stand;
        eventManager.OnPetBecameMainCharacter -= Sit;
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
