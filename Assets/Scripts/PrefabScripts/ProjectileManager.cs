using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Vector2.Distance(startPos, rb.transform.position) >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ADD IF STATEMENT HERE TO CHECK FOR SCRIPT OR TAG OF ANOTHER TANK TO ADD DAMAGE USING GE COMPONENT FOR ANOTHER OBJECT

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            rb.velocity = Vector2.zero;
            //ADD SOME FX HERE
            Destroy(gameObject);
        }
    }
}
