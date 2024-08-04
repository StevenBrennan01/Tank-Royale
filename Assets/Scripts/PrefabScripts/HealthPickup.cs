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
            healthIncreaseCR = StartCoroutine(IncreaseHealth(collision));

            if (healVFX != null)
            {
                healVFX.gameObject.SetActive(true);
            }

            //Destroy(this.gameObject);
        }
    }

    private IEnumerator IncreaseHealth(Collider2D collision)
    {
        collision.gameObject.GetComponent<HealthManager>().IncreaseHealth(healAmount);
        yield return null;
    }
}
