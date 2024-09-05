using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetInventory : MonoBehaviour
{
    [SerializeField]
    private Image pet1Image;
    [SerializeField]
    private TMP_Text pet1NameText;
    [SerializeField]
    private TMP_Text pet1LevelText;
    [SerializeField]
    private TMP_Text pet1ExpText;
    [SerializeField]
    private TMP_Text pet1HPText;

    [SerializeField]
    private Image pet2Image;
    [SerializeField]
    private TMP_Text pet2NameText;
    [SerializeField]
    private TMP_Text pet2LevelText;
    [SerializeField]
    private TMP_Text pet2ExpText;
    [SerializeField]
    private TMP_Text pet2HPText;

    [SerializeField]
    private Image pet3Image;
    [SerializeField]
    private TMP_Text pet3NameText;
    [SerializeField]
    private TMP_Text pet3LevelText;
    [SerializeField]
    private TMP_Text pet3ExpText;
    [SerializeField]
    private TMP_Text pet3HPText;

    [SerializeField]
    private Image pet4Image;
    [SerializeField]
    private TMP_Text pet4NameText;
    [SerializeField]
    private TMP_Text pet4LevelText;
    [SerializeField]
    private TMP_Text pet4ExpText;
    [SerializeField]
    private TMP_Text pet4HPText;

    private void OnEnable()
    {
        Render();
    }

    private void Render()
    {
        if (PetSpaceData.Pets.Count > 0)
        {
            pet1Image.sprite = PetSpaceData.Pets[0].Icon;
            pet1NameText.text = PetSpaceData.Pets[0].Name;
            pet1ExpText.text = new string(PetSpaceData.Pets[0].CurrentExp + "/" + PetSpaceData.Pets[0].MaxExp);
            pet1HPText.text = new string(PetSpaceData.Pets[0].CurrentHP + "/" + PetSpaceData.Pets[0].MaxHP);
            pet1LevelText.text = new string(PetSpaceData.Pets[0].CurrentLevel.ToString());
        }
        if (PetSpaceData.Pets.Count > 1)
        {
            pet2Image.sprite = PetSpaceData.Pets[1].Icon;
            pet2NameText.text = PetSpaceData.Pets[1].Name;
            pet2ExpText.text = new string(PetSpaceData.Pets[1].CurrentExp + "/" + PetSpaceData.Pets[1].MaxExp);
            pet2HPText.text = new string(PetSpaceData.Pets[1].CurrentHP + "/" + PetSpaceData.Pets[1].MaxHP);
            pet2LevelText.text = new string(PetSpaceData.Pets[1].CurrentLevel.ToString());
        }
        if (PetSpaceData.Pets.Count > 2)
        {
            pet3Image.sprite = PetSpaceData.Pets[2].Icon;
            pet3NameText.text = PetSpaceData.Pets[2].Name;
            pet3ExpText.text = new string(PetSpaceData.Pets[2].CurrentExp + "/" + PetSpaceData.Pets[2].MaxExp);
            pet3HPText.text = new string(PetSpaceData.Pets[2].CurrentHP + "/" + PetSpaceData.Pets[2].MaxHP);
            pet3LevelText.text = new string(PetSpaceData.Pets[2].CurrentLevel.ToString());
        }
        if (PetSpaceData.Pets.Count > 3)
        {
            pet4Image.sprite = PetSpaceData.Pets[3].Icon;
            pet4NameText.text = PetSpaceData.Pets[3].Name;
            pet4ExpText.text = new string(PetSpaceData.Pets[3].CurrentExp + "/" + PetSpaceData.Pets[3].MaxExp);
            pet4HPText.text = new string(PetSpaceData.Pets[3].CurrentHP + "/" + PetSpaceData.Pets[3].MaxHP);
            pet4LevelText.text = new string(PetSpaceData.Pets[3].CurrentLevel.ToString());
        }
    }
}
