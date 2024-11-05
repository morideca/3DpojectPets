using UnityEngine;

public class Ball : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private bool monsterIn = false;
    private int id;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionStay(Collision other)
    {
        if (monsterIn == true && other.gameObject.CompareTag("Player"))
        {
            MonsterSpace.Instance.PickedUpMonster(id);
            Destroy(gameObject);
        }

        if (other.gameObject.TryGetComponent(out BaseHumanoid monster))
        {
            id = monster.ID;
            Destroy(monster.gameObject);

            Material material = meshRenderer.material;
            material.color = Color.red;
            monsterIn = true;
        }
        else if (!monsterIn) Destroy(gameObject);
    }
}
