using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private bool monsterIn = false;
    private int id;

    public static event Action<int> pickedUpTheBall;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out BaseHumanoid monster))
        {
            id = monster.ID;
            Destroy(monster.gameObject);

            Material material = meshRenderer.material;
            material.color = Color.red;
            monsterIn = true;
        }
        else if (!monsterIn) Destroy(gameObject);
        if (monsterIn == true && other.gameObject.CompareTag("Player"))
        {
            pickedUpTheBall?.Invoke(id);
            Debug.Log(id);
            Destroy(gameObject);
        }
    }
}
