using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderScript : MonoBehaviour
{

    public Transform player;
    public bool canMove = true;
    public int damage;
    public float moveSpeed;
    public float health;
    public float freezeTime;
    public GameObject healthBar;
    public GameObject damageBar;
    public float shotCooldown;
    public GameObject web;

    public Animator enemyAnim;

    private float shotTimer;
    private bool canShoot = true;
    private bool facingRight = false;
    private Vector3 healthBarScale;
    private float healthPercent;

    // Start is called before the first frame update
    void Start()
    {
        healthBarScale = healthBar.transform.localScale;
        healthPercent = healthBarScale.x / health;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shotTimer = shotCooldown;
    }

    void UpdateHealthbar(){
        healthBarScale.x = healthPercent * health;
        healthBar.transform.localScale = healthBarScale;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCanShoot();
    }


    private void CheckCanShoot()
    {
        if (shotTimer <= 0)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
            shotTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (player != null && canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (canShoot)
            {
                Shoot();
            }

            if ((facingRight && player.position.x < transform.position.x) || (!facingRight && player.position.x > transform.position.x)) Flip();
        }
    }

    void Shoot()
    {
        Instantiate(web, transform.position, Quaternion.identity);
        shotTimer = shotCooldown;
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        //healthBar.transform.Rotate(0f, 180f, 0f);
        //damageBar.transform.Rotate(0f, 180f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<PlayerScript>().TakeDamage(damage);
            StartCoroutine(StopAfterAttack());
        }
    }

    IEnumerator StopAfterAttack()
    {
        canMove = false;
        yield return new WaitForSeconds(freezeTime);
        canMove = true;
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                StartCoroutine("Death");
            }
            UpdateHealthbar();
        }
    }

    public void Freeze()
    {
        StartCoroutine("IsFreeze");
    }

    IEnumerator IsFreeze()
    {
        canMove = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.6f, 1, 1);
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        canMove = true;
    }

    IEnumerator Death()
    {
        moveSpeed = 0;
        Destroy(healthBar);
        Destroy(damageBar);
        enemyAnim.SetTrigger("Death");
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }

}
