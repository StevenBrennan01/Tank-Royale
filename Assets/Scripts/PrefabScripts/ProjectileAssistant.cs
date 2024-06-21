using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAssistant : MonoBehaviour
{
    #region Inspector Header and Spacing
    [Header("                                                  -= Projectile Manager =-")]
    [Header("                                   (If projectile hits wall it will auto destroy)")]
    [Space(15)]
    #endregion

    //[SerializeField] private float damageValue;

    private Rigidbody2D rb;
    private Vector2 startPos;

    private float maxRange = 100f;

    //[SerializeField] private ParticleSystem collisionEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(startPos, rb.transform.position) >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ADD IF STATEMENT HERE TO CHECK FOR SCRIPT OR TAG OF ANOTHER TANK TO ADD DAMAGE USING GET COMPONENT FOR ANOTHER OBJECT

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            //collisionEffect.Play();

            rb.velocity = Vector2.zero;
            Destroy(gameObject);

            //                            (INTENSITY, FOR TIME)
            //CinemachineShake.Instance.CameraShake(1f, .25f);
        }
    }
}
