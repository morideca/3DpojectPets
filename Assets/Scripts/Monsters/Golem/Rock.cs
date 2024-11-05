using System;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public event Action<List<GameObject>> rockExplosed;

    [SerializeField]
    private ParticleSystem effect;
    [SerializeField]
    private bool isPet;
    private bool thrown = false;

    [SerializeField]
    private LayerMask layers;
    private void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Instantiate(effect, transform.position, Quaternion.identity);
                CheckForHit();
                Destroy(gameObject);
            }
            else if (isPet && (collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag("Enemy")))
            {
                Instantiate(effect, transform.position, Quaternion.identity);
                CheckForHit();
                Destroy(gameObject);
            }
            else if (!isPet && collision.gameObject.CompareTag("Pet"))
            {
                Instantiate(effect, transform.position, Quaternion.identity);
                CheckForHit();
                Destroy(gameObject);
            }

        }
    }

    private void CheckForHit()
    {
        var colls = Physics.OverlapSphere(transform.position, 1.5f, layers);
        List<GameObject> hitTargets = new List<GameObject>();
        foreach (var coll in colls)
        {
            hitTargets.Add(coll.gameObject);
        }
        if (hitTargets[0] != null) rockExplosed?.Invoke(hitTargets);
    }

    public void ThrownOn() => thrown = true;
}
