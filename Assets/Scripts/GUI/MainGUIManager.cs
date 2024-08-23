using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class MainGUIManager : MonoBehaviour
{
    [SerializeField]
    private Image petCell1;
    [SerializeField] 
    private Image petCell2;
    [SerializeField] 
    private Image petCell3;
    [SerializeField] 
    private Image petCell4;

    [SerializeField]
    private Image petIcon1;
    [SerializeField]
    private Image petIcon2;
    [SerializeField]
    private Image petIcon3;
    [SerializeField]
    private Image petIcon4;

    private Image currentSlot;

    private void OnEnable()
    {
        MonsterSpace.setFirstSlot += SetFirstSlot;
        MonsterSpace.setSecondSlot += SetSecondSlot;
        MonsterSpace.setThirdSlot += SetThirdSlot;
        MonsterSpace.setFourthSlot += SetFourthSlot;
        PlayerThrowBall.setCurrentPetSlot += SetCurrentPetSlot;
    }

    private void OnDisable()
    {
        MonsterSpace.setFirstSlot -= SetFirstSlot;
        MonsterSpace.setSecondSlot -= SetSecondSlot;
        MonsterSpace.setThirdSlot -= SetThirdSlot;
        MonsterSpace.setFourthSlot -= SetFourthSlot;
        PlayerThrowBall.setCurrentPetSlot -= SetCurrentPetSlot;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void SetCurrentPetSlot(int number)
    {
        if (currentSlot != null) currentSlot.sprite = Resources.Load<Sprite>("GUI/Cell/NonSelected");
        switch (number)
        {
            case 0:
                currentSlot.sprite = Resources.Load<Sprite>("GUI/Cell/NonSelected");
                currentSlot = null;
                break;
            case 1:
                currentSlot = petCell1;
                currentSlot.sprite = Resources.Load<Sprite>("GUI/Cell/Selected");
                break;
            case 2:
                currentSlot = petCell2;
                currentSlot.sprite = Resources.Load<Sprite>("GUI/Cell/Selected");
                break;
            case 3:
                currentSlot = petCell3;
                currentSlot.sprite = Resources.Load<Sprite>("GUI/Cell/Selected");
                break;
            case 4:
                currentSlot = petCell4;
                currentSlot.sprite = Resources.Load<Sprite>("GUI/Cell/Selected");
                break;
        }
    }

    private void SetFirstSlot(Monster pet)
    {
        petIcon1.sprite = pet.Icon;
    }

    private void SetSecondSlot(Monster pet)
    {
        petIcon2.sprite = pet.Icon;
    }

    private void SetThirdSlot(Monster pet)
    {
        petIcon3.sprite = pet.Icon;
    }

    private void SetFourthSlot(Monster pet)
    {
        petIcon4.sprite = pet.Icon;
    }
}
