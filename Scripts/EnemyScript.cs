using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public Transform player;
    public bool canMove = true;
    public int damage;
    public float moveSpeed;
    public float health;
    public float freezeTime;

    public GameObject healthBar;
    public GameObject damageBar;

    public GameObject heart;
    public int changeDropHeart;

    public Animator enemyAnim;

    private Vector3 healthBarScale;
    private float healthPercent;
    
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
        if (player != null && canMove){
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            player.GetComponent<PlayerScript>().TakeDamage(damage);
            StartCoroutine(FreezeMovement(freezeTime));
        }
    }

    IEnumerator FreezeMovement(float freezeTime){
        canMove = false;
        yield return new WaitForSeconds(freezeTime);
        canMove = true;
    }

    public void TakeDamage(float damage){
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
        DropHeart();
        Destroy(gameObject);
    }

    private void DropHeart()
    {
        if (Random.Range(0, changeDropHeart) == 1)
        {
            Instantiate(heart, transform.position, Quaternion.identity);
        }
    }
}
