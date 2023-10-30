using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaProiettile : MonoBehaviour
{
    [SerializeField] private float lifeDuration = 4f;
    private float lifeTimer;

    void Start()
    {
        lifeTimer = lifeDuration;
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
