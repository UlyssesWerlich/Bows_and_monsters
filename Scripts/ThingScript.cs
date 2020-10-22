using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingScript : MonoBehaviour
{
    private bool canMove = true;
    private bool facingRight = false;
    private Vector3 healthBarScale;
    private float healthPercent;

    public Animator enemyAnim;

    public Transform player;
    public GameObject healthBar;
    public GameObject damageBar;
    public float moveSpeed;
    public int damage;
    public float freezeTime;

    public float health;

    // Start is called before the first frame update
    void Start()
    {
        healthBarScale = healthBar.transform.localScale;
        healthPercent = healthBarScale.x / health;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void UpdateHealthbar(){
        healthBarScale.x = healthPercent * health;
        healthBar.transform.localScale = healthBarScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (player != null && canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if ((facingRight && player.position.x < transform.position.x) || (!facingRight && player.position.x > transform.position.x)) Flip();
        }
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
