using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectileAssistant : MonoBehaviour
{
    private HealthManager healthManager_SCR;

    #region Inspector Header and Spacing
    [Header("                                                 -= Projectile Manager =-")]
    [Header("                                   (If projectile hits wall it will auto destroy)")]
    [Space(15)]
    #endregion

    [SerializeField] private float damageValue;
    //[SerializeField] private float damageDelay;

    private Rigidbody2D rb;
    private Vector2 startPos;

    private Coroutine GiveDamage_CR;

    //[SerializeField] private ParticleSystem collisionEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HealthManager>() != null)
        {
            GiveDamage_CR = StartCoroutine(DealDamage(collision));

            //rb.velocity = Vector2.zero;

            ObjectPoolManager.ReturnObjectToPool(this.gameObject); //INSTEAD OF DESTROYING, RETURN TO POOL AND RECYCLE
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            //collisionEffect.Play();

            rb.velocity = Vector2.zero;

            ObjectPoolManager.ReturnObjectToPool(this.gameObject); //INSTEAD OF DESTROYING, RETURN TO POOL AND RECYCLE
        }
    }

    private IEnumerator DealDamage(Collision2D collision)
    {
        //healthManager_SCR.DealDamage(damageValue);
        collision.gameObject.GetComponent<HealthManager>().DealDamage(damageValue);
        yield return null;
    }
}
