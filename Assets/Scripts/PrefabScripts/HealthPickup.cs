using System.Collections;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private HealthManager healthManager_SCR;

    private Coroutine healthIncreaseCR;
    private Coroutine reinstateHealthPickup;

    [SerializeField] private float healAmount;
    [SerializeField] private float respawnDelay = 4f;

    private void Awake()
    {
        healthManager_SCR = GetComponent<HealthManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            healthIncreaseCR = StartCoroutine(IncreaseHealth(collision));
            reinstateHealthPickup = StartCoroutine(ReinstateHP());
        }
    }

    private IEnumerator ReinstateHP()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);
        gameObject.SetActive(true);
    }

    private IEnumerator IncreaseHealth(Collider2D collision)
    {
        collision.gameObject.GetComponent<HealthManager>().IncreaseHealth(healAmount);
        yield return null;
    }
}