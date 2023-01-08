using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Health
{
    public Transform player;
    public float speed = 5f;

    public Transform agroPoint;
    public float agroRange = 5f;
    public LayerMask playerLayer;

    public float attackRange = 3f;
    bool inRangeOfAttack = false;
    [SerializeField] int attackDamage = 10;
    Vector3 attackOffset = new Vector3(0f, 0f ,0f);

    public float attackCooldown = 0.5f;
    [HideInInspector] public float lastAttackTime = 0f;

    bool inAgroRange = false;
    bool MustChasePlayer = false;

    // Enemy Patrol
    public Transform castPos;
    const string LEFT = "left";
    const string RIGHT = "right";

    public string facingDirection;

    Vector3 baseScale;
    public float baseCastDist = 0.1f;


    Rigidbody2D rb;
    Animator anim;
    public Healthbar healthbar;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        baseScale = transform.localScale;

        facingDirection = RIGHT;
    }

    private void FixedUpdate()
    {
        
        if(player != null)
        {
            PlayerSpotted();

            if (inAgroRange && MustChasePlayer == true)
            {

                Debug.Log("AGRO: TRUE");

                if (facingDirection == LEFT && transform.position.x < player.position.x)
                {
                    ChangeFacingDirection(RIGHT);
                }
                else if (facingDirection == RIGHT && transform.position.x > player.position.x)
                {
                    ChangeFacingDirection(LEFT);
                }

                ChasePlayer();

            }
            else
            {
                // Do Something Patrol or Stop
                Debug.Log("AGRO: FALSE");

                Patrol();

                if (IsHittingWall() || IsNearEdge())
                {

                    if (facingDirection == LEFT)
                    {
                        ChangeFacingDirection(RIGHT);
                    }
                    else if (facingDirection == RIGHT)
                    {
                        ChangeFacingDirection(LEFT);
                    }
                }
            }
        }
    }

    void ChasePlayer()
    {
        // In Range Of Attack
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            inRangeOfAttack = true;

            // Stop Goblin From Moving
            rb.velocity = new Vector2(0f, rb.position.y);

            // Check if you can attack base on cooldown
            if (Time.time - lastAttackTime > attackCooldown)
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

        // Chase Player if not enough range of attack
        if (!inRangeOfAttack)
        {
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }

    void PlayerSpotted()
    {
        // Check If Player is in enemy agro range
        if(Vector2.Distance(player.position, rb.position) <= agroRange)
        {
            // Is Enemy Going To Fall or Close In Edge
            inAgroRange = true;

            if (IsNearEdge())
            {
                MustChasePlayer = false;
            }

        } else
        {
            inAgroRange = false;
            MustChasePlayer = true;
        }

        Debug.Log("MUSTCHASE: " + MustChasePlayer.ToString());
    }

    bool IsHittingWall()
    {
        bool val = false;

        float castDist = baseCastDist;

        if(facingDirection == LEFT)
        {
            castDist = -baseCastDist;
        } else
        {
            castDist = baseCastDist;
        }
        // Determine the target destination based on the cast distance
        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.blue);

        if(Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground"))) 
        {
            val = true;
        } 
        else
        {
            val = false;
        }

        return val;
    }

    bool IsNearEdge()
    {
        bool val = true;

        float castDist = baseCastDist;

        // Determine the target destination based on the cast distance
        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist; // Shoot Down For Cheking Edges

        Debug.DrawLine(castPos.position, targetPos, Color.red);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = false;
        }
        else
        {
            val = true;
        }

        return val;
    }

    void Patrol()
    {
        float moveSpeed = speed;
        if (facingDirection == LEFT)
        {
            moveSpeed = -speed;
        }

        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void ChangeFacingDirection(string direction)
    {
        Vector3 newScale = baseScale;

        if(direction == LEFT)
        {
            newScale.x = -baseScale.x;
        } else if(direction == RIGHT)
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;

        facingDirection = direction;
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
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, playerLayer);

        if(colInfo != null)
        {
            Player playerScript = colInfo.GetComponent<Player>();
            playerScript.ReduceHealth(attackDamage);

            Animator playerAnim = colInfo.GetComponent<Animator>();
            playerAnim.SetTrigger("Hurt");

            // Update Heathbar
            healthbar.SetHealthBar(playerScript.GetHealth());
        }
           
    }



}
