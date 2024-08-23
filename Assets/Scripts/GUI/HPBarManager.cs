using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarManager : MonoBehaviour
{
    private Slider slider;

    private HealthManager healthManager;

    [SerializeField]
    private GameObject hpBarPrefub;

    [SerializeField]
    private Transform hpBarTransform;

    private GameObject hpBar;



    private void OnDisable()
    {
        healthManager.wasDamaged -= ChangeValue;
    }

    private void Start()
    {
        InstantiateHealthBar();
        healthManager = GetComponent<HealthManager>();
        int maxHealth = healthManager.MaxHealth;
        SetMaxValue(maxHealth);
        healthManager.wasDamaged += ChangeValue;
    }

    private void InstantiateHealthBar()
    {
        hpBar = Instantiate(hpBarPrefub, hpBarTransform.position, Quaternion.identity, gameObject.transform);
        slider = hpBar.GetComponentInChildren<Slider>();
    }

    private void SetMaxValue(int maxValue)
    {
        slider.maxValue = maxValue;
    }

    private void ChangeValue(int value)
    {
        slider.value = value;
    }

    private void Update()
    {
        
    }
}
