using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private HealthManager healthManager_SCR;

    private Coroutine healthIncreaseCR;

    [SerializeField] private float healAmount;
    [SerializeField] private ParticleSystem healVFX;

    private void Awake()
    {
        healthManager_SCR = GetComponent<HealthManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayVFX();

            healthIncreaseCR = StartCoroutine(IncreaseHealth(collision));

            Destroy(this.gameObject);
        }
    }

    private void PlayVFX()
    {
        if (healVFX != null)
        {
            healVFX.Play(this.transform);
        }
    }

    private IEnumerator IncreaseHealth(Collider2D collision)
    {
        collision.gameObject.GetComponent<HealthManager>().IncreaseHealth(healAmount);
        yield return null;
    }
}