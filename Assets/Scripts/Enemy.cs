using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Health
{
    public Transform player;

    public float speed = 5f;

    public bool isFlipped = false;

    public Transform agroPoint;
    public float agroRange = 5f;
    public LayerMask playerLayer;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        PlayerSpotted();
    }

    void ChasePlayer()
    {
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if(transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if(transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    void PlayerSpotted()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(agroPoint.position, agroRange, playerLayer);

        foreach (Collider2D players in hitPlayers)
        {
            LookAtPlayer();
            ChasePlayer();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (agroPoint == null) return;

        Gizmos.DrawWireSphere(agroPoint.position, agroRange);
    }



}
