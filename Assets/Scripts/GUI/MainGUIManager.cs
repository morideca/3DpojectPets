using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class MainGUIManager : MonoBehaviour
{
    [SerializeField]
    private List<Image> petCells = new List<Image>();
    [SerializeField]
    private List<Image> petIcons = new List<Image>();

    private Image currentSlot;

    [SerializeField]
    private GameObject petInventory;

    private void Awake()
    {
        MonsterSpace.setPetSlot += SetPetSlot;
        PlayerThrowBall.setCurrentPetSlot += SetCurrentPetSlot;
    }

    private void OnEnable()
    {
        Render();
    }

    private void OnDisable()
    {
        MonsterSpace.setPetSlot -= SetPetSlot;
        PlayerThrowBall.setCurrentPetSlot -= SetCurrentPetSlot;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (petInventory.activeSelf) petInventory.SetActive(false);
            else petInventory.SetActive(true);
        }
    }

    private void SetCurrentPetSlot(int index)
    {
        if (currentSlot != null) currentSlot.sprite = Resources.Load<Sprite>("GUI/Cell/NonSelected");

        if (index == 0)
        {
            currentSlot.sprite = Resources.Load<Sprite>("GUI/Cell/NonSelected");
            currentSlot = null;
        }
        else
        {
            currentSlot = petCells[index - 1];
            currentSlot.sprite = Resources.Load<Sprite>("GUI/Cell/Selected");
        }
    }

    private void Render()
    {
        int i = 0;
        foreach (var petIcon in petIcons)
        {
            petIcon.sprite = PetSpaceData.Pets[i].Icon;
            i++;
        }
    }

    private void SetPetSlot(Pet pet, int i)
    {
        petIcons[i - 1].sprite = pet.Icon;
    }
}
