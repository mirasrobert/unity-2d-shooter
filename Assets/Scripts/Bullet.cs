using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int bulletDamage = 10;
    [SerializeField] float speed = 20f;

    public GameObject impactEffect;

    GameObject impactPrefabClone;

    Rigidbody2D rb2d;
    

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //Add force to our bullet 
        if (PlayerMovement.getIsFacingRight() == true)
        {
            // Right
            rb2d.velocity = transform.right * speed;
        }
        else if (PlayerMovement.getIsFacingRight() == false)
        {
            // Left
            rb2d.velocity = -transform.right * speed;
        }

    }

    private void Update()
    {
        // This will destroy bullets if does not collide with anything
        StartCoroutine("waitAndDestroy", 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        // We hit enemy
        if(enemy != null)
        {
            enemy.ReduceHealth(bulletDamage);
        }

        // As Long as not collectible then destroy bullet
        if(!collision.gameObject.CompareTag("Collectible"))
        {
            impactPrefabClone = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(impactPrefabClone, 0.5f);
            Destroy(gameObject);
        }
    }

    private IEnumerator waitAndDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if(impactPrefabClone != null)
            Destroy(impactPrefabClone);
        if(gameObject != null)
            Destroy(gameObject);
    }

}
