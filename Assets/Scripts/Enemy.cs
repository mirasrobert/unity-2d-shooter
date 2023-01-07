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

    public float attackRange = 3f;
    bool inRangeOfAttack = false;
    [SerializeField] int attackDamage = 10;

    public float attackCooldown = 0.5f;
    public float lastAttackTime = 0f;

    Rigidbody2D rb;
    Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        if (player != null)
            PlayerSpotted();
    }

    void ChasePlayer()
    {
        // In Range Of Attack
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            inRangeOfAttack = true;
            rb.velocity = new Vector2(0f, rb.position.y);

            // Check if you can attack base on cooldown
            if(Time.time - lastAttackTime > attackCooldown)
            {
                lastAttackTime = Time.time;
                // Attack
                anim.SetTrigger("Attack");
            }

        }
        else
        {
            inRangeOfAttack = false;
        }

        if (!inRangeOfAttack)
        {
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
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
        Collider2D[] playersCollided = Physics2D.OverlapCircleAll(agroPoint.position, agroRange, playerLayer);

        // Chase Player If in Range
        foreach (Collider2D players in playersCollided)
        {
            LookAtPlayer();
            ChasePlayer();
        }
        
    }


    void OnDrawGizmosSelected()
    {
        if (agroPoint == null) return;

        Gizmos.DrawWireSphere(agroPoint.position, agroRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Will be used in animation event
    public void DamagePlayer()
    {
        player.GetComponent<Player>().ReduceHealth(attackDamage);
    }



}
