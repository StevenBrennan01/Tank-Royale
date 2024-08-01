using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private HealthManager healthManager_SCR;

    private Coroutine healthIncreaseCR;

    [SerializeField] private float healAmount;

    private void Awake()
    {
        healthManager_SCR = GetComponent<HealthManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HealthManager>() != null)
        {
            healthIncreaseCR = StartCoroutine(IncreaseHealth(collision));
        }
    }

    private IEnumerator IncreaseHealth(Collision2D collision)
    {
        collision.gameObject.GetComponent<HealthManager>().IncreaseHealth(healAmount);
        yield return null;
    }
}
