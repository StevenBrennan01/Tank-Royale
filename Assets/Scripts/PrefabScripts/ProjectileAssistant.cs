using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectileAssistant : MonoBehaviour
{
    #region Inspector Header and Spacing
    [Header("                                                 -= Projectile Manager =-")]
    [Header("                                   (If projectile hits wall it will auto destroy)")]
    [Space(15)]
    #endregion

    [SerializeField] private float damageValue;

    private Rigidbody2D rb;
    private Vector2 startPos;

    //[SerializeField] private ParticleSystem collisionEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HealthManager>() != null)
        {
            //give damage

            Debug.Log("HealthManager found on gameobject");

            rb.velocity = Vector2.zero;
            Destroy(this.gameObject);

            Debug.Log("Object destroyed");
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            //collisionEffect.Play();

            rb.velocity = Vector2.zero;
            Destroy(this.gameObject);
        }
    }
}
