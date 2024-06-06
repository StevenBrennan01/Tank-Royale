using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public float verticalDir;

    public Vector2 moveDir;

    [SerializeField] private float moveSpeed = 100f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TankVertical()
    {
        rb.AddForce(transform.up * verticalDir * moveSpeed, ForceMode2D.Force);
    }

    public void tempMovement()
    {
        rb.AddForce(moveDir * moveSpeed, ForceMode2D.Force);
    }

    public void stopMovement()
    {
        
    }
}